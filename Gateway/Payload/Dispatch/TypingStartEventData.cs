using Gracie.ETF;
using Gracie.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Gracie.Gateway.Payload.Dispatch
{
    /// <summary>
    /// Sent when a user starts typing in a channel
    /// https://discord.com/developers/docs/topics/gateway#typing-start
    /// </summary>
    public class TypingStartEventData
    {
        /// <summary>
        /// id of the channel
        /// </summary>
        [EtfProperty("channel_id")]
        public BigInteger ChannelId { get; set; }

        /// <summary>
        /// id of the guild
        /// </summary>
        [EtfProperty("guild_id")]
        public BigInteger GuildId { get; set; }

        /// <summary>
        /// id of the user
        /// </summary>
        [EtfProperty("user_id")]
        public BigInteger UserId { get; set; }

        /// <summary>
        /// unix time (in seconds) of when the user started typing
        /// </summary>
        [EtfProperty("timestamp")]
        public int Timestamp { get; set; }

        /// <summary>
        /// the member who started typing if this happened in a guild
        /// </summary>
        [EtfProperty("member")]
        public GuildMember Member { get; set; }
    }
}
