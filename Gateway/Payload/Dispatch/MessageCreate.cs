using Gracie.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Gateway.Payload.Dispatch
{
    public class MessageCreate : Payload
    {
        public MessageCreate(int? sequenceNumber = null, string eventName = null) : base(Opcode.Dispatch, sequenceNumber, eventName)
        {
        }
    }
}
