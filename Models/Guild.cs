using Gracie.ETF;
using System.Collections.Generic;

namespace Gracie.Models
{
    /// <summary>
    /// https://discord.com/developers/docs/resources/guild#guild-object
    /// </summary>
    public class Guild
    {
        /// <summary>
        /// guild id
        /// </summary>
        [EtfProperty("id")]
        public ulong Id { get; private set; }

        /// <summary>
        /// true if this guild is unavailable due to an outage
        /// </summary>
        [EtfProperty("unavailable")]
        public bool Unavailable { get; private set; }

        /// <summary>
        /// guild name (2-100 characters, excluding trailing and leading whitespace)
        /// </summary>
        [EtfProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// icon hash
        /// https://discord.com/developers/docs/reference#image-formatting
        /// </summary>
        [EtfProperty("icon")]
        public string Icon { get; private set; }

        /// <summary>
        /// splash hash
        /// https://discord.com/developers/docs/reference#image-formatting
        /// </summary>
        [EtfProperty("splash")]
        public string Splash { get; private set; }

        /// <summary>
        /// discovery splash hash; only present for guilds with the "DISCOVERABLE" feature
        /// https://discord.com/developers/docs/reference#image-formatting
        /// </summary>
        [EtfProperty("discovery_splash")]
        public string DiscoverySplash { get; private set; }

        /// <summary>
        /// true if the user is the owner of the guild
        /// </summary>
        [EtfProperty("owner")]
        public bool? Owner { get; private set; }

        /// <summary>
        /// id of owner
        /// </summary>
        [EtfProperty("owner_id")]
        public ulong OwnerId { get; private set; }

        /// <summary>
        /// total permissions for the user in the guild (excludes overrides)
        /// </summary>
        [EtfProperty("permissions")]
        public int Permissions { get; private set; }

        /// <summary>
        /// voice region id for the guild
        /// https://discord.com/developers/docs/resources/voice#voice-region-object
        /// </summary>
        [EtfProperty("region")]
        public string Region { get; private set; }

        /// <summary>
        /// id of afk channel
        /// </summary>
        [EtfProperty("afk_channel_id")]
        public ulong? AfkChannelId { get; private set; }

        /// <summary>
        /// afk timeout in seconds
        /// </summary>
        [EtfProperty("afk_timeout")]
        public int AfkTimeout { get; private set; }

        /// <summary>
        /// verification level required for the guild
        /// https://discord.com/developers/docs/resources/guild#guild-object-verification-level
        /// </summary>
        [EtfProperty("verification_level")]
        public int VerificationLevel { get; private set; }

        /// <summary>
        /// default message notifications level
        /// https://discord.com/developers/docs/resources/guild#guild-object-default-message-notification-level
        /// </summary>
        [EtfProperty("default_message_notifications")]
        public int DefaultMessageNotifications { get; private set; }

        /// <summary>
        /// explicit content filter level
        /// https://discord.com/developers/docs/resources/guild#guild-object-explicit-content-filter-level
        /// </summary>
        [EtfProperty("explicit_content_filter")]
        public int ExplicitContentFilter { get; private set; }

        /// <summary>
        /// roles in the guild
        /// </summary>
        [EtfProperty("roles")]
        public List<Role> Roles { get; private set; }

        /// <summary>
        /// custom guild emojis
        /// </summary>
        [EtfProperty("emojis")]
        public List<Emoji> Emojis { get; private set; }

        /// <summary>
        /// enabled guild features
        /// https://discord.com/developers/docs/resources/guild#guild-object-guild-features4
        /// </summary>
        [EtfProperty("features")]
        public List<string> Features { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("mfa_level")]
        public int MfaLevel { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("application_id")]
        public ulong? ApplicationId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("widget_enabled")]
        public bool? WidgetEnabled { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("widget_channel_id")]
        public ulong? WidgetChannelId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("system_channel_id")]
        public ulong? SystemChannelId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("system_channel_flags")]
        public int SystemChannelFlags { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("rules_channel_id")]
        public ulong? RulesChannelId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("joined_at")]
        public string JoinedAt { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("large")]
        public bool? Large { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("member_count")]
        public int MemberCount { get; private set; }
    }
}
