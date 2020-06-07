using Gracie.Core;
using Gracie.Gateway.EventData;
using Gracie.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Gracie.Gateway
{
    public class GatewayClient : BaseClient
    {
        private readonly ILogger<GatewayClient> logger;
        private readonly ObjectDeserializer objectDeserializer;
        private readonly Dictionary<string, DispatchEventHandler> dispatchEventHandlers;

        public GatewayClient(ClientWebSocket webSocket, SemaphoreSlim semaphoreSlim, int bufferSize, ILogger<GatewayClient> logger, ObjectDeserializer objectDeserializer, int? taskTimeout = null) : base(webSocket, semaphoreSlim, bufferSize, logger, taskTimeout)
        {
            NewPayloadReceived += GatewayClient_NewPayloadReceived;
            WebSocketClosed += WebSocket_WebSocketClosed;
            this.logger = logger;
            this.objectDeserializer = objectDeserializer;
            dispatchEventHandlers = BuildDispatchEventDict();
        }

        private Task WebSocket_WebSocketClosed(object sender, WebSocketReceiveResult receiveResult, byte[] buffer, CancellationToken cancellation = default)
        {
            logger.LogError("Connection closed", receiveResult.CloseStatusDescription);
            return Task.CompletedTask;
        }

        private async Task GatewayClient_NewPayloadReceived(object sender, Dictionary<string, object> result, int? opcode, int? sequenceNumber, string eventName, CancellationToken cancellationToken)
        {
            switch ((Opcode)opcode)
            {
                case Opcode.Dispatch:
                    await DeserializeDispatch(result, eventName, cancellationToken);
                    break;
                case Opcode.Hello:
                    if (HelloReceived != null)
                    {
                        var payload = objectDeserializer.Deserialize<GatewayDataPayload<Hello>>(result);
                        await HelloReceived(this, payload, cancellationToken);
                    }
                    break;
                case Opcode.HeartbeatACK:
                    if (HeartbeatAckReceived != null)
                    {
                        var payload = objectDeserializer.Deserialize<GatewayPayload>(result);
                        await HeartbeatAckReceived(this, payload, cancellationToken);
                    }
                    break;
                case Opcode.Heartbeat:
                    if (HeartbeatReceived != null)
                    {
                        var payload = objectDeserializer.Deserialize<GatewayDataPayload<int?>>(result);
                        await HeartbeatReceived(this, payload, cancellationToken);
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

        private async Task DeserializeDispatch(Dictionary<string, object> data, string eventName, CancellationToken cancellationToken)
        {
            if (dispatchEventHandlers.TryGetValue(eventName, out var eventHandler))
            {
                await eventHandler(data, cancellationToken);
            }
            else
            {
                logger?.Log(LogLevel.Information, "Received unknown dispatch event: {eventName}", eventName);
            }
        }

        // we pass in a function that returns the event rather the event itself otherwise
        // the event is always null as its passed in before we subscribe
        private DispatchEventHandler BuildEvent<T>(Func<GatewayEventHandler<T>> eventGetter)
        {
            return async (Dictionary<string, object> data, CancellationToken cancellationToken) =>
            {
                var eventToCall = eventGetter();
                if (eventToCall != null)
                {
                    var payload = objectDeserializer.Deserialize<T>(data);
                    await eventToCall(this, payload, cancellationToken);
                }
            };
        }

        private Dictionary<string, DispatchEventHandler> BuildDispatchEventDict()
        {
            return new Dictionary<string, DispatchEventHandler>
            {
                { "READY",                  BuildEvent(() => ReadyReceived) },
                { "TYPING_START",           BuildEvent(() => TypingStartReceived) },
                { "GUILD_CREATE",           BuildEvent(() => GuildCreateReceived) },
                { "MESSAGE_CREATE",         BuildEvent(() => MessageCreateReceived) },
                { "VOICE_SERVER_UPDATE",    BuildEvent(() => VoiceServerUpdateReceived) },
                { "VOICE_STATE_UPDATE",     BuildEvent(() => VoiceStateUpdateReceived) },
                { "USER_UPDATE",            BuildEvent(() => UserUpdateReceived) },
                { "WEBHOOKS_UPDATE",        BuildEvent(() => WebhooksUpdateReceived) },
                { "PRESENCE_UPDATE",        BuildEvent(() => PresenceUpdateReceived) },
                { "MESSAGE_DELETE",         BuildEvent(() => MessageDeleteReceived) },
                { "MESSAGE_DELETE_BULK",    BuildEvent(() => MessageDeleteBulkReceived) },
                { "MESSAGE_UPDATE",         BuildEvent(() => MessageUpdateReceived) },
                { "MESSAGE_REACTION_ADD",   BuildEvent(() => MessageReactionAddReceived) }
            };
        }

        private delegate Task DispatchEventHandler(Dictionary<string, object> data, CancellationToken cancellationToken);
        public delegate Task GatewayEventHandler<T>(object sender, T payload, CancellationToken cancellationToken);

        public event GatewayEventHandler<GatewayDataPayload<Hello>> HelloReceived;
        public event GatewayEventHandler<GatewayDataPayload<int?>> HeartbeatReceived;
        public event GatewayEventHandler<GatewayPayload> HeartbeatAckReceived;

        public event GatewayEventHandler<GatewayDataPayload<Ready>> ReadyReceived;
        public event GatewayEventHandler<GatewayDataPayload<TypingStart>> TypingStartReceived;
        public event GatewayEventHandler<GatewayDataPayload<Guild>> GuildCreateReceived;
        public event GatewayEventHandler<GatewayDataPayload<Message>> MessageCreateReceived;
        public event GatewayEventHandler<GatewayDataPayload<VoiceServerUpdate>> VoiceServerUpdateReceived;
        public event GatewayEventHandler<GatewayDataPayload<VoiceState>> VoiceStateUpdateReceived;
        public event GatewayEventHandler<GatewayDataPayload<User>> UserUpdateReceived;
        public event GatewayEventHandler<GatewayDataPayload<WebhooksUpdate>> WebhooksUpdateReceived;
        public event GatewayEventHandler<GatewayDataPayload<PresenceUpdate>> PresenceUpdateReceived;
        public event GatewayEventHandler<GatewayDataPayload<MessageDelete>> MessageDeleteReceived;
        public event GatewayEventHandler<GatewayDataPayload<MessageDeleteBulk>> MessageDeleteBulkReceived;
        public event GatewayEventHandler<GatewayDataPayload<Message>> MessageUpdateReceived;
        public event GatewayEventHandler<GatewayDataPayload<MessageReactionAdd>> MessageReactionAddReceived;
    }
}
