using Gracie.ETF;

namespace Gracie.Gateway
{
    public class VoiceServerUpdate
    {
        [EtfProperty("token")]
        public string Token { get; set; }

        [EtfProperty("guild_id")]
        public ulong GuildId { get; set; }

        [EtfProperty("endpoint")]
        public string Endpoint { get; set; }
    }
}
