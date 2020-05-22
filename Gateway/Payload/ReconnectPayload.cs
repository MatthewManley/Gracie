using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public class ReconnectPayload : Payload
    {
        public ReconnectPayload() : base(Opcode.Reconnect)
        {

        }
    }
}
