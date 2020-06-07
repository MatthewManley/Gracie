using Gracie.ETF;
using System.Collections.Generic;

namespace Gracie.Gateway.EventData
{
    public class MessageDeleteBulk
    {
        /// <summary>
        /// the ids of the messages
        /// </summary>
        [EtfProperty("ids")]
        public List<ulong> Ids { get; set; }

        /// <summary>
        /// the id of the channel
        /// </summary>
        [EtfProperty("channel_id")]
        public ulong ChannelId { get; set; }

        /// <summary>
        /// the id of the guild
        /// </summary>
        [EtfProperty("guild_id")]
        public ulong? GuildId { get; set; }
    }
}
