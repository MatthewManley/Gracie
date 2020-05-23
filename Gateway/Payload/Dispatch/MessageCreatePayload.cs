using Gracie.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Gateway.Payload.Dispatch
{
    public class MessageCreatePayload : DataPayload<Message>
    {
        public MessageCreatePayload(Message data, int? sequenceNumber = null, string eventName = null) : base(data, Opcode.Dispatch, sequenceNumber, eventName)
        {
        }
    }
}
