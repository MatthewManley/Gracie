using Gracie.ETF;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public class HeartbeatPayload : SerializablePayload
    {
        private readonly int? lastSequenceNumber;

        public HeartbeatPayload(int? lastSequenceNumber) : base(Opcode.Heartbeat)
        {
            this.lastSequenceNumber = lastSequenceNumber;
        }

        public override bool HasData => true;

        public override int SerializeData(byte[] buffer, int position)
        {
            if (lastSequenceNumber.HasValue)
            {
                return ETFSerializer.SerializeIntegerExt(buffer, position, lastSequenceNumber.Value);
            }
            return ETFSerializer.SerializeAtomExt(buffer, position, "nil");
        }
    }
}
