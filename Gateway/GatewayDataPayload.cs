using Gracie.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Gateway
{
    public class GatewayDataPayload<T> : DataPayload<T>
    {
        public Opcode GatewayOpcode
        {
            get => (Opcode)Opcode;
            set => Opcode = (byte)value;
        }
    }
}
