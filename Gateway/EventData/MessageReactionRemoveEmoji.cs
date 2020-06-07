using Gracie.ETF;
using Gracie.Models;

namespace Gracie.Gateway.EventData
{
    /// <summary>
    /// Sent when a bot removes all instances of a given emoji from the reactions of a message.
    /// https://discord.com/developers/docs/topics/gateway#message-reaction-remove-emoji
    /// </summary>
    public class MessageReactionRemoveEmoji
    {
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

        /// <summary>
        /// the id of the message
        /// </summary>
        [EtfProperty("message_id")]
        public ulong MessageId { get; set; }

        /// <summary>
        /// the emoji that was removed
        /// </summary>
        [EtfProperty("emoji")]
        public Emoji Emoji { get; set; }
    }
}
