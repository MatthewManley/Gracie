using Gracie.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    /// <summary>
    /// Sent on connection to the websocket. Defines the heartbeat interval that the client should heartbeat to.
    /// https://discord.com/developers/docs/topics/gateway#hello
    /// </summary>
    public class HelloPayload : Payload
    {
        /// <summary>
        /// the interval (in milliseconds) the client should heartbeat with
        /// </summary>
        [PropertyName("heartbeat_interval")]
        public int HeartbeatInterval { get; private set; }

        public HelloPayload(int? sequenceNumber, string eventName) : base(Opcode.Hello, sequenceNumber, eventName) { }
    }
}
