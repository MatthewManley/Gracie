using System.Collections.Generic;
using System.Numerics;

namespace Gracie.Models
{
    /// <summary>
    /// https://discord.com/developers/docs/resources/emoji#emoji-object
    /// </summary>
    public class Emoji
    {
        /// <summary>
        /// emoji id
        /// </summary>
        [PropertyName("id")]
        public BigInteger Id { get; private set; }

        /// <summary>
        /// emoji name
        /// </summary>
        [PropertyName("name")]
        public string Name { get; private set; }

        /// <summary>
        /// list of role object ids
        /// </summary>
        [PropertyName("roles")]
        public List<Role> Roles { get; }

        /// <summary>
        /// user that created this emoji
        /// </summary>
        [PropertyName("user")]
        public User User { get; private set; }

        /// <summary>
        /// whether this emoji must be wrapped in colons
        /// </summary>
        [PropertyName("require_colons")]
        public bool? RequireColons { get; private set; }

        /// <summary>
        /// whether this emoji is managed
        /// </summary>
        [PropertyName("managed")]
        public bool? Managed { get; private set; }

        /// <summary>
        /// whether this emoji is animated
        /// </summary>
        [PropertyName("animated")]
        public bool? Animated { get; private set; }

        /// <summary>
        /// whether this emoji can be used, may be false due to loss of Server Boosts
        /// </summary>
        [PropertyName("available")]
        public bool? Available { get; private set; }
    }
}
