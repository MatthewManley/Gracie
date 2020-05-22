using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Gateway.Payload.Dispatch
{
    public class GuildCreateEventPayload : Payload
    {
        public GuildCreateEventPayload(int? sequenceNumber = null, string eventName = null) : base(Opcode.Dispatch, sequenceNumber, eventName)
        {
        }
    }
}
