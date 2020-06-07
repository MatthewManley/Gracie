using Gracie.ETF;

namespace Gracie.Core
{
    public class Payload
    {
        [EtfProperty("op")]
        public byte Opcode { get; set; }

        [EtfProperty("s")]
        public int? SequenceNumber { get; set; }

        [EtfProperty("t")]
        public string EventName { get; set; }
    }
}
