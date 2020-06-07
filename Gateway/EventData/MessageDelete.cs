using Gracie.ETF;

namespace Gracie.Gateway.EventData
{
    public class MessageDelete
    {
        /// <summary>
        /// the id of the message
        /// </summary>
        [EtfProperty("id")]
        public ulong Id { get; set; }

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
