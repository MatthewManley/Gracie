using Gracie.Core;
using Gracie.Gateway;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Voice
{
    public class VoiceClient : BaseClient
    {
        private readonly ObjectDeserializer objectDeserializer;

        public VoiceClient(ClientWebSocket webSocket, SemaphoreSlim semaphoreSlim, ILogger<VoiceClient> logger, int bufferSize, ObjectDeserializer objectDeserializer, int? taskTimeout = null): base(webSocket, semaphoreSlim, bufferSize, logger, taskTimeout)
        {
            WebSocketClosed += WebSocket_WebSocketClosed;
            NewPayloadReceived += VoiceClient_NewPayloadReceived;
            this.objectDeserializer = objectDeserializer;
        }

        private Task WebSocket_WebSocketClosed(object sender, WebSocketReceiveResult receiveResult, byte[] buffer, CancellationToken cancellation = default)
        {
            return Task.CompletedTask;
        }

        private Task VoiceClient_NewPayloadReceived(object sender, System.Collections.Generic.Dictionary<string, object> payload, int? opcode, int? sequenceNumber, string eventName, CancellationToken cancellationToken)
        {
            switch ((Opcode)opcode)
            {
                case Opcode.Identify:
                    break;
                case Opcode.SelectProtocol:
                    break;
                case Opcode.Ready:
                    break;
                case Opcode.Heartbeat:
                    break;
                case Opcode.SessionDescription:
                    break;
                case Opcode.Speaking:
                    break;
                case Opcode.HeartbeatAck:
                    break;
                case Opcode.Resume:
                    break;
                case Opcode.Hello:
                    break;
                case Opcode.Resumed:
                    break;
                case Opcode.ClientDisconnect:
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
