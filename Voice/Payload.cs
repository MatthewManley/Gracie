using Gracie.ETF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voice
{
    public class Payload
    {
        [EtfProperty("op")]
        public Opcode Opcode { get; set; }

        [EtfProperty("s")]
        public int? SequenceNumber { get; set; }

        [EtfProperty("t")]
        public string EventName { get; set; }
    }
}
