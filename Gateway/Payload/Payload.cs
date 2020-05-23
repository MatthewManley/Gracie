using Gracie.ETF;
using Gracie.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public abstract class Payload
    {
        [EtfProperty("op")]
        public Opcode Opcode { get; }

        [EtfProperty("s")]
        public int? SequenceNumber { get; }

        [EtfProperty("t")]
        public string EventName { get; }
        protected Payload(Opcode opcode, int? sequenceNumber = null, string eventName = null)
        {
            Opcode = opcode;
            SequenceNumber = sequenceNumber;
            EventName = eventName;
        }
    }
}
