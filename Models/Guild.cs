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
        [PropertyName("id")]
        public ulong Id { get; private set; }

        /// <summary>
        /// true if this guild is unavailable due to an outage
        /// </summary>
        [PropertyName("unavailable")]
        public bool Unavailable { get; private set; }

        /// <summary>
        /// guild name (2-100 characters, excluding trailing and leading whitespace)
        /// </summary>
        [PropertyName("name")]
        public string Name { get; private set; }

        /// <summary>
        /// icon hash
        /// https://discord.com/developers/docs/reference#image-formatting
        /// </summary>
        [PropertyName("icon")]
        public string Icon { get; private set; }

        /// <summary>
        /// splash hash
        /// https://discord.com/developers/docs/reference#image-formatting
        /// </summary>
        [PropertyName("splash")]
        public string Splash { get; private set; }

        /// <summary>
        /// discovery splash hash; only present for guilds with the "DISCOVERABLE" feature
        /// https://discord.com/developers/docs/reference#image-formatting
        /// </summary>
        [PropertyName("discovery_splash")]
        public string DiscoverySplash { get; private set; }

        /// <summary>
        /// true if the user is the owner of the guild
        /// </summary>
        [PropertyName("owner")]
        public bool? Owner { get; private set; }

        /// <summary>
        /// id of owner
        /// </summary>
        [PropertyName("owner_id")]
        public ulong OwnerId { get; private set; }

        /// <summary>
        /// total permissions for the user in the guild (excludes overrides)
        /// </summary>
        [PropertyName("permissions")]
        public int Permissions { get; private set; }

        /// <summary>
        /// voice region id for the guild
        /// https://discord.com/developers/docs/resources/voice#voice-region-object
        /// </summary>
        [PropertyName("region")]
        public string Region { get; private set; }

        /// <summary>
        /// id of afk channel
        /// </summary>
        [PropertyName("afk_channel_id")]
        public ulong? AfkChannelId { get; private set; }

        /// <summary>
        /// afk timeout in seconds
        /// </summary>
        [PropertyName("afk_timeout")]
        public int AfkTimeout { get; private set; }

        /// <summary>
        /// verification level required for the guild
        /// https://discord.com/developers/docs/resources/guild#guild-object-verification-level
        /// </summary>
        [PropertyName("verification_level")]
        public int VerificationLevel { get; private set; }

        /// <summary>
        /// default message notifications level
        /// https://discord.com/developers/docs/resources/guild#guild-object-default-message-notification-level
        /// </summary>
        [PropertyName("default_message_notifications")]
        public int DefaultMessageNotifications { get; private set; }

        /// <summary>
        /// explicit content filter level
        /// https://discord.com/developers/docs/resources/guild#guild-object-explicit-content-filter-level
        /// </summary>
        [PropertyName("explicit_content_filter")]
        public int ExplicitContentFilter { get; private set; }

        /// <summary>
        /// roles in the guild
        /// </summary>
        [PropertyName("roles")]
        public List<Role> Roles { get; private set; }

        /// <summary>
        /// custom guild emojis
        /// </summary>
        [PropertyName("emojis")]
        public List<Emoji> Emojis { get; private set; }

        /// <summary>
        /// enabled guild features
        /// https://discord.com/developers/docs/resources/guild#guild-object-guild-features4
        /// </summary>
        [PropertyName("features")]
        public List<string> Features { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("mfa_level")]
        public int MfaLevel { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("application_id")]
        public ulong? ApplicationId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("widget_enabled")]
        public bool? WidgetEnabled { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("widget_channel_id")]
        public ulong? WidgetChannelId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("system_channel_id")]
        public ulong? SystemChannelId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("system_channel_flags")]
        public int SystemChannelFlags { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("rules_channel_id")]
        public ulong? RulesChannelId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("joined_at")]
        public string JoinedAt { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("large")]
        public bool? Large { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("member_count")]
        public int MemberCount { get; private set; }
    }
}
