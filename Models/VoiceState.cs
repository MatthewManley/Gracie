using Gracie.ETF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Models
{
    //TODO
    /// <summary>
    /// https://discord.com/developers/docs/resources/voice#voice-state-object
    /// </summary>
    public class VoiceState
    {
        [EtfProperty("guild_id")]
        public ulong? GuildId { get; set; }

        [EtfProperty("channel_id")]
        public ulong? ChannelId { get; set; }

        [EtfProperty("user_id")]
        public ulong? UserId { get; set; }

        [EtfProperty("member")]
        public GuildMember Member { get; set; }

        [EtfProperty("session_id")]
        public string SessionId { get; set; }

        [EtfProperty("deaf")]
        public bool? Deaf { get; set; }

        [EtfProperty("mute")]
        public bool? Mute { get; set; }

        [EtfProperty("self_deaf")]
        public bool? SelfDeaf { get; set; }

        [EtfProperty("self_mute")]
        public bool? SelfMute { get; set; }

        [EtfProperty("self_stream")]
        public bool? SelfStream { get; set; }

        [EtfProperty("suppress")]
        public bool? Supress { get; set; }
    }
}
