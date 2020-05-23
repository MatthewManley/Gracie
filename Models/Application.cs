using Gracie.ETF;

namespace Gracie.Models
{
    public class Application
    {
        [EtfProperty("flags")]
        public int? Flags { get; private set; }

        [EtfProperty("id")]
        public ulong Id { get; private set; }
    }
}
