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
    public class TypingStartEventPayload : Payload
    {
        public TypingStartEventPayload(int? sequenceNumber, string eventName) : base(Opcode.Dispatch, sequenceNumber, eventName) { }

        /// <summary>
        /// id of the channel
        /// </summary>
        [PropertyName("channel_id")]
        public BigInteger ChannelId { get; set; }

        /// <summary>
        /// id of the guild
        /// </summary>
        [PropertyName("guild_id")]
        public BigInteger GuildId { get; set; }

        /// <summary>
        /// id of the user
        /// </summary>
        [PropertyName("user_id")]
        public BigInteger UserId { get; set; }

        /// <summary>
        /// unix time (in seconds) of when the user started typing
        /// </summary>
        [PropertyName("timestamp")]
        public int Timestamp { get; set; }

        /// <summary>
        /// the member who started typing if this happened in a guild
        /// </summary>
        [PropertyName("member")]
        public GuildMember Member { get; set; }
    }
}
