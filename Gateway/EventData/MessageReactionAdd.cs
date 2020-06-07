using System;
using System.Collections.Generic;
using System.Text;
using Gracie.ETF;
using Gracie.Models;

namespace Gracie.Gateway.EventData
{
    /// <summary>
    /// https://discord.com/developers/docs/topics/gateway#message-reaction-add
    /// </summary>
    public class MessageReactionAdd
    {
        /// <summary>
        /// the id of the user
        /// </summary>
        [EtfProperty("user_id")]
        public ulong UserId { get; set; }

        /// <summary>
        /// the id of the channel
        /// </summary>
        [EtfProperty("channel_id")]
        public ulong ChannelId { get; set; }

        /// <summary>
        /// the id of the message
        /// </summary>
        [EtfProperty("message_id")]
        public ulong MessageId { get; set; }

        /// <summary>
        /// the id of the guild
        /// </summary>
        [EtfProperty("guild_id")]
        public ulong? GuildId { get; set; }

        /// <summary>
        /// the member who reacted if this happened in a guild
        /// </summary>
        [EtfProperty("member")]
        public GuildMember Member { get; set; }

        /// <summary>
        /// the emoji used to react
        /// </summary>
        [EtfProperty("emoji")]
        public Emoji Emoji { get; set; }
    }
}
