using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public class VoiceStateUpdatePayload : Payload
    {
        public VoiceStateUpdatePayload() : base(Opcode.VoiceStateUpdate)
        {

        }
    }
}
