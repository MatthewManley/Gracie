using Gracie.ETF;
using Gracie.Gateway;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Gracie.Core
{
    public class BaseClient : SemaphoreWebSocket
    {
        private readonly ILogger<BaseClient> logger;

        public BaseClient(ClientWebSocket webSocket, SemaphoreSlim semaphoreSlim, int bufferSize, ILogger<BaseClient> logger, int? taskTimeout = null) : base(webSocket, semaphoreSlim, bufferSize, taskTimeout)
        {
            this.logger = logger;
            this.MessageReceived += WebSocket_MessageReceived;
        }

        private async Task WebSocket_MessageReceived(object sender, WebSocketReceiveResult receiveResult, byte[] buffer, CancellationToken cancellation = default)
        {
            if (!receiveResult.EndOfMessage)
            {
                //TODO: Custom exception or allow messages to be parsed in multiple packets
                throw new Exception("Didn't recieve entire message in one packet");
            }
            if (receiveResult.MessageType == WebSocketMessageType.Text)
            {
                throw new Exception("Expected binary message type");
            }
            await DeserializeMessage(buffer, cancellation);
        }

        private async Task DeserializeMessage(byte[] buffer, CancellationToken cancellationToken)
        {
            var result = BeginParse(buffer);
            var opcode = NumToInt(result["op"]);
            var sequenceNumber = NumToInt(result["s"]);
            var eventName = (string)result["t"];

            if (cancellationToken.IsCancellationRequested)
                return;

            if (NewSequenceNumber != null && sequenceNumber.HasValue)
            {
                await NewSequenceNumber(this, sequenceNumber.Value, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested)
                return;

            if (NewPayloadReceived != null)
            {
                await NewPayloadReceived(this, result, opcode, sequenceNumber, eventName, cancellationToken);
            }
        }

        public async Task SendPayload(Payload payload, int bufferSize, CancellationToken cancellationToken)
        {
            var buffer = new byte[bufferSize];
            var length = ETFSerializer.ObjectToTerm(buffer, 0, payload);
            var segment = new ArraySegment<byte>(buffer, 0, length);
            await this.SendAsync(segment, WebSocketMessageType.Binary, true, cancellationToken);
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

        public delegate Task NewSequenceNumberHandler(object sender, int sequenceNumber, CancellationToken cancellationToken);
        public event NewSequenceNumberHandler NewSequenceNumber;

        public delegate Task NewPayloadHandler(object sender, Dictionary<string, object> payload, int? opcode, int? sequenceNumber, string eventName, CancellationToken cancellationToken);
        public event NewPayloadHandler NewPayloadReceived;
    }
}
