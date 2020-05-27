using Gracie.ETF;
using Gracie.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
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
