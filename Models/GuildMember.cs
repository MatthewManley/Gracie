using System.Collections.Generic;
using System.Numerics;

namespace Gracie.Models
{
    /// <summary>
    /// https://discord.com/developers/docs/resources/guild#guild-member-object
    /// </summary>
    public class GuildMember
    {
        /// <summary>
        /// the user this guild member represents
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// this users guild nickname
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// list of role object ids
        /// </summary>
        public List<BigInteger> Roles { get; set; }

        /// <summary>
        /// when the user joined the guild
        /// </summary>
        public string JoinedAt { get; set; }

        /// <summary>
        /// when the user started boosting the guild
        /// </summary>
        public string PremiumSince { get; set; }

        /// <summary>
        /// whether the user is deafened in voice channels
        /// </summary>
        public bool Deaf { get; set; }

        /// <summary>
        /// whether the user is muted in voice channels
        /// </summary>
        public bool Mute { get; set; }

        public BigInteger? HoistedRole { get; set; }
    }
}
