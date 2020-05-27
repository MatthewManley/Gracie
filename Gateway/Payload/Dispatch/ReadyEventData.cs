using System;
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
    public class ReadyEventData
    {
        [EtfProperty("application")]
        public Application Application { get; set; }

        /// <summary>
        /// the guilds the user is in
        /// </summary>
        [EtfProperty("guilds")]
        public IList<Guild> Guilds { get; set; }

        /// <summary>
        /// gateway version
        /// </summary>
        [EtfProperty("v")]
        public int GatewayVersion { get; set; }

        /// <summary>
        /// information about the user including email
        /// </summary>
        [EtfProperty("user")]
        public User User { get; set; }

        /// <summary>
        /// used for resuming connections
        /// </summary>
        [EtfProperty("session_id")]
        public string SessionId { get; set; }
    }
}
