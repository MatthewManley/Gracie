using Gracie.ETF;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public class HeartbeatPayload : DataPayload<int?>
    {
        public HeartbeatPayload(int? lastSequenceNumber = null) : base(lastSequenceNumber, Opcode.Heartbeat) { }
    }
}
