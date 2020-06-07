using Gracie.ETF;

namespace Gracie.Gateway.EventData
{
    /// <summary>
    /// Sent when a user explicitly removes all reactions from a message
    /// https://discord.com/developers/docs/topics/gateway#message-reaction-remove-all
    /// </summary>
    public class MessageReactionRemoveAll
    {
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
    }
}
