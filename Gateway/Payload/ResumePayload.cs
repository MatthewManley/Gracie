using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public class ResumePayload : Payload
    {
        public ResumePayload() : base(Opcode.Resume)
        {

        }
    }
}
