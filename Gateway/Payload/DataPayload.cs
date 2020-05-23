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
        //TODO: will need to pass in the opcode as well, it shouldn't always be dispatch
        public DataPayload(T data, Opcode opcode, int? sequenceNumber = null, string eventName = null) : base(opcode, sequenceNumber, eventName)
        {
            Data = data;
        }
    }
}
