using Gracie.ETF;
using Gracie.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public class DataPayload<T> : Payload
    {
        [EtfProperty("d", true)]
        public T Data { get; set; }
    }
}
