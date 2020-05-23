﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gracie.ETF;
using Gracie.Models;
using Microsoft.Extensions.Logging;

namespace Gracie.Gateway.Payload.Dispatch
{
    /// <summary>
    /// https://discord.com/developers/docs/topics/gateway#ready
    /// </summary>
    public class ReadyEventPayload : Payload
    {
        public ReadyEventPayload(int? sequenceNumber, string eventName) : base(Opcode.Dispatch, sequenceNumber, eventName)
        {
        }

        [EtfProperty("application")]
        public Application Application { get; private set; }

        /// <summary>
        /// the guilds the user is in
        /// </summary>
        [EtfProperty("guilds")]
        public IList<Guild> Guilds { get; private set; }

        /// <summary>
        /// gateway version
        /// </summary>
        [EtfProperty("v")]
        public int GatewayVersion { get; private set; }

        /// <summary>
        /// information about the user including email
        /// </summary>
        [EtfProperty("user")]
        public User User { get; private set; }

        /// <summary>
        /// used for resuming connections
        /// </summary>
        [EtfProperty("session_id")]
        public string SessionId { get; private set; }
    }
}
