using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public abstract class Payload
    {
        public Opcode Opcode { get; }
        public int? SequenceNumber { get; }
        public string EventName { get; }
        protected Payload(Opcode opcode, int? sequenceNumber = null, string eventName = null)
        {
            Opcode = opcode;
            SequenceNumber = sequenceNumber;
            EventName = eventName;
        }
    }
}
