using Gracie.ETF;

namespace Gracie.Core
{
    public class DataPayload<T> : Payload
    {
        [EtfProperty("d", true)]
        public T Data { get; set; }
    }
}
