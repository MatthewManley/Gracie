using Gracie.ETF;
using Gracie.Gateway.Payload;
using Gracie.Gateway.Payload.Dispatch;
using Gracie.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gracie.Gateway
{
    public class GatewayClient
    {
        private readonly ClientWebSocket webSocket;
        private readonly ILogger<GatewayClient> logger;
        private readonly ObjectDeserializer objectDeserializer;

        // TODO: make these configurable
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(10);
        private const int DeserializeAndRunTimeout = 20000;
        private readonly bool ShouldTimeout = false;

        public GatewayClient(ClientWebSocket webSocket, ILogger<GatewayClient> logger, ObjectDeserializer objectDeserializer)
        {
            this.webSocket = webSocket;
            this.logger = logger;
            this.objectDeserializer = objectDeserializer;
        }

        public async Task Connect(Uri uri, CancellationToken cancellationToken = default)
        {
            await webSocket.ConnectAsync(uri, cancellationToken);
        }

        public async Task Recieve(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var buffer = new byte[1024 * 1024];
                var segment = new ArraySegment<byte>(buffer);
                var recieveResult = await webSocket.ReceiveAsync(segment, cancellationToken);
                if (recieveResult.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogError("Connection closed", recieveResult.CloseStatusDescription);
                    return;
                }
                if (!recieveResult.EndOfMessage)
                {
                    //TODO: Custom exception or allow messages to be parsed in multiple packets
                    throw new Exception("Didn't recieve entire message in one packet");
                }
                if (recieveResult.MessageType == WebSocketMessageType.Text)
                {
                    throw new Exception("Expected binary message type");
                }
                await semaphore.WaitAsync(cancellationToken);

                _ = Task.Run(async () =>
                {
                    CancellationToken token;
                    if (ShouldTimeout)
                    {
                        token = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(10000).Token).Token;
                    }
                    else
                    {
                        token = cancellationToken;
                    }
                    try
                    {
                        await DeserializeMessage(buffer, token);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
            }
        }

        private async Task DeserializeMessage(byte[] buffer, CancellationToken cancellationToken)
        {
            // Here is a nice list of gateway events: https://discord.com/developers/docs/topics/gateway#commands-and-events-gateway-events
            var result = BeginParse(buffer);
            var opcode = (Opcode)result["op"];
            var sequenceNumber = NumToInt(result["s"]);
            var eventName = (string)result["t"];
            if (NewSequenceNumber != null && sequenceNumber.HasValue)
            {
                await NewSequenceNumber(this, sequenceNumber.Value);
            }
            switch (opcode)
            {
                case Opcode.Dispatch:
                    await DeserializeDispatch(result, eventName);
                    break;
                case Opcode.Hello:
                    if (HelloReceived != null)
                    {
                        var payload = objectDeserializer.Deserialize<DataPayload<HelloData>>(result);
                        await HelloReceived(this, payload);
                    }
                    break;
                case Opcode.HeartbeatACK:
                    if (HeartbeatAckReceived != null)
                    {
                        var payload = objectDeserializer.Deserialize<Payload.Payload>(result);
                        await HeartbeatAckReceived(this, payload);
                    }
                    break;
                case Opcode.Heartbeat:
                    if (HeartbeatReceived != null)
                    {
                        var payload = objectDeserializer.Deserialize<DataPayload<int?>>(result);
                        await HeartbeatReceived(this, payload);
                    }
                    break;
                case Opcode.PresenceUpdate:
                case Opcode.VoiceStateUpdate:
                case Opcode.Resume:
                case Opcode.Reconnect:
                case Opcode.RequestGuildMembers:
                case Opcode.InvalidSession:
                default:
                    logger.LogInformation("Recieved unkown opcode {opcode}", opcode);
                    break;
            }
        }

        private int? NumToInt(object value)
        {
            if (value is null)
            {
                return null;
            }
            else if (value is int intVal)
            {
                return intVal;
            }
            else if (value is byte byteVal)
            {
                return Convert.ToInt32(byteVal);
            }
            throw new NotImplementedException();
        }


        private async Task DeserializeDispatch(Dictionary<string, object> data, string eventName)
        {
            switch (eventName)
            {
                case "READY":
                    if (ReadyReceived != null)
                    {
                        var payload = objectDeserializer.Deserialize<DataPayload<ReadyEventData>>(data);
                        await ReadyReceived(this, payload);
                    }
                    break;
                case "TYPING_START":
                    if (TypingStartReceived != null)
                    {
                        var payload = objectDeserializer.Deserialize<DataPayload<TypingStartEventData>>(data);
                        await TypingStartReceived(this, payload);
                    }
                    break;
                case "GUILD_CREATE":
                    if (GuildCreateReceived != null)
                    {
                    }
                    break;
                case "MESSAGE_CREATE":
                    if (MessageCreateReceived != null)
                    {
                        var payload = objectDeserializer.Deserialize<DataPayload<Message>>(data);
                        await MessageCreateReceived(this, payload);
                    }
                    break;
                default:
                    logger?.Log(LogLevel.Information, "Received unknown dispatch event: {eventName}", eventName);
                    break;
            }
        }

        public async Task Send(Payload.Payload payload)
        {
            var buffer = new byte[4096]; // 4096 is the max payload length that can be sent to the gateway
            var length = ETFSerializer.ObjectToTerm(buffer, 0, payload);
            var segment = new ArraySegment<byte>(buffer, 0, length);
            await webSocket.SendAsync(segment, WebSocketMessageType.Binary, true, CancellationToken.None);
        }


        //TODO: move to etf namespace
        public static Dictionary<string, object> BeginParse(byte[] buffer)
        {

            if (buffer[0] != ETFConstants.FORMAT_VERSION ||
                buffer[1] != ETFConstants.MAP_EXT)
            {
                //TODO: custom exception
                throw new Exception("Invalid ETF format");
            }
            int position = 2;
            return ETFDeserializer.DeserializeMap(buffer, ref position);
        }

        public delegate Task HelloReceivedHandler(object sender, DataPayload<HelloData> payload);
        public event HelloReceivedHandler HelloReceived;

        public delegate Task HeartbeatReceiedHandler(object sender, Payload.Payload payload);
        public event HeartbeatReceiedHandler HeartbeatReceived;

        public delegate Task HeartbeatAckReceivedHandler(object sender, Payload.Payload payload);
        public event HeartbeatAckReceivedHandler HeartbeatAckReceived;

        public delegate Task NewSequenceNumberHandler(object sender, int sequenceNumber);
        public event NewSequenceNumberHandler NewSequenceNumber;

        public delegate Task ReadyReceivedHandler(object sender, DataPayload<ReadyEventData> readyPayload);
        public event ReadyReceivedHandler ReadyReceived;

        public delegate Task TypingStartReceivedHandler(object sender, DataPayload<TypingStartEventData> typingStartEventPayload);
        public event TypingStartReceivedHandler TypingStartReceived;

        public delegate Task GuildCreateReceivedHandler(object sender, DataPayload<TypingStartEventData> typingStartEventPayload);
        public event GuildCreateReceivedHandler GuildCreateReceived;

        public delegate Task MessageCreateReceivedHandler(object sender, DataPayload<Message> messageCreatePayload);
        public event MessageCreateReceivedHandler MessageCreateReceived;
    }
}
