using Gracie.ETF;

namespace Gracie.Gateway.EventData
{
    public class WebhooksUpdate
    {
        [EtfProperty("guild_id")]
        public ulong GuildId { get; set; }

        [EtfProperty("channel_id")]
        public ulong ChannelId { get; set; }
    }
}
