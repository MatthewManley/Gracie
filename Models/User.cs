using System.Numerics;

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
        [PropertyName("id")]
        public BigInteger Id { get; private set; }

        /// <summary>
        /// the user's username, not unique across the platform
        /// </summary>
        [PropertyName("username")]
        public string Username { get; private set; }

        /// <summary>
        /// the user's 4-digit discord-tag
        /// </summary>
        [PropertyName("discriminator")]
        public string Discriminator { get; private set; }

        /// <summary>
        /// the user's avatar hash
        /// </summary>
        [PropertyName("avatar")]
        public string Avatar { get; private set; }

        /// <summary>
        /// whether the user belongs to an OAuth2 application
        /// </summary>
        [PropertyName("bot")]
        public bool? Bot { get; private set; }

        /// <summary>
        /// whether the user is an Official Discord System user (part of the urgent message system)
        /// </summary>
        [PropertyName("system")]
        public bool? System { get; private set; }

        /// <summary>
        /// whether the user has two factor enabled on their account
        /// </summary>
        [PropertyName("mfa_enabled")]
        public bool? MfaEnabled { get; private set; }

        /// <summary>
        /// the user's chosen language option
        /// </summary>
        [PropertyName("locale")]
        public string Locale { get; private set; }

        /// <summary>
        /// whether the email on this account has been verified
        /// </summary>
        [PropertyName("verified")]
        public bool? Verified { get; private set; }

        /// <summary>
        /// the user's email
        /// </summary>
        [PropertyName("email")]
        public string Email { get; private set; }

        /// <summary>
        /// the flags on a user's account
        /// <see cref="https://discord.com/developers/docs/resources/user#user-object-user-flags"/>
        /// </summary>
        [PropertyName("flags")]
        public int? Flags { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("premium_type")]
        public int? PremiumType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyName("public_flags")]
        public int? PublicFlags { get; private set; }
    }
}
