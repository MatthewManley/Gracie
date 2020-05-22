using System.Numerics;

namespace Gracie.Models
{
    public class Application
    {
        [PropertyName("flags")]
        public int? Flags { get; private set; }

        [PropertyName("id")]
        public BigInteger Id { get; private set; }
    }
}
