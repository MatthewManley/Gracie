using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public class HeartbeatAckPayload : Payload
    {
        public HeartbeatAckPayload(int? sequenceNumber, string eventName) : base(Opcode.HeartbeatACK, sequenceNumber, eventName) {}
    }
}
