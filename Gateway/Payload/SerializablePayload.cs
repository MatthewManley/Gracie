using Gracie.ETF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public abstract class SerializablePayload : Payload
    {
        public SerializablePayload(Opcode opcode, int? sequenceNumber = null, string eventName = null) : base(opcode, sequenceNumber, eventName)
        {
        }

        public int Serialize(byte[] buffer)
        {
            int position = 0;
            buffer[position] = ETFConstants.FORMAT_VERSION;
            position += 1;
            var items = new List<(string, ETFSerializer.SerializeItem)>();

            items.Add(("op", SerializeItemHelpers.SerializeSmallIntegerExt((byte)Opcode)));

            if (HasData)
            {
                items.Add(("d", SerializeData));
            }
            position += ETFSerializer.SerializeMapExt(buffer, position, items);

            return position;
        }

        public abstract int SerializeData(byte[] buffer, int position);
        public abstract bool HasData { get; }
    }
}
