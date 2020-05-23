using Gracie.ETF;

namespace Gracie.Models
{
    /// <summary>
    /// https://discord.com/developers/docs/resources/user#user-object
    /// </summary>
    public class User
    {
        /// <summary>
        /// the user's id
        /// </summary>
        [EtfProperty("id")]
        public ulong Id { get; private set; }

        /// <summary>
        /// the user's username, not unique across the platform
        /// </summary>
        [EtfProperty("username")]
        public string Username { get; private set; }

        /// <summary>
        /// the user's 4-digit discord-tag
        /// </summary>
        [EtfProperty("discriminator")]
        public string Discriminator { get; private set; }

        /// <summary>
        /// the user's avatar hash
        /// </summary>
        [EtfProperty("avatar")]
        public string Avatar { get; private set; }

        /// <summary>
        /// whether the user belongs to an OAuth2 application
        /// </summary>
        [EtfProperty("bot")]
        public bool? Bot { get; private set; }

        /// <summary>
        /// whether the user is an Official Discord System user (part of the urgent message system)
        /// </summary>
        [EtfProperty("system")]
        public bool? System { get; private set; }

        /// <summary>
        /// whether the user has two factor enabled on their account
        /// </summary>
        [EtfProperty("mfa_enabled")]
        public bool? MfaEnabled { get; private set; }

        /// <summary>
        /// the user's chosen language option
        /// </summary>
        [EtfProperty("locale")]
        public string Locale { get; private set; }

        /// <summary>
        /// whether the email on this account has been verified
        /// </summary>
        [EtfProperty("verified")]
        public bool? Verified { get; private set; }

        /// <summary>
        /// the user's email
        /// </summary>
        [EtfProperty("email")]
        public string Email { get; private set; }

        /// <summary>
        /// the flags on a user's account
        /// <see cref="https://discord.com/developers/docs/resources/user#user-object-user-flags"/>
        /// </summary>
        [EtfProperty("flags")]
        public int? Flags { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("premium_type")]
        public int? PremiumType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [EtfProperty("public_flags")]
        public int? PublicFlags { get; private set; }
    }
}
