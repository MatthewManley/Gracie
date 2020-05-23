using Gracie.ETF;
using System.Collections.Generic;

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
        [EtfProperty("id")]
        public ulong Id { get; private set; }

        /// <summary>
        /// emoji name
        /// </summary>
        [EtfProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// list of role object ids
        /// </summary>
        [EtfProperty("roles")]
        public List<Role> Roles { get; }

        /// <summary>
        /// user that created this emoji
        /// </summary>
        [EtfProperty("user")]
        public User User { get; private set; }

        /// <summary>
        /// whether this emoji must be wrapped in colons
        /// </summary>
        [EtfProperty("require_colons")]
        public bool? RequireColons { get; private set; }

        /// <summary>
        /// whether this emoji is managed
        /// </summary>
        [EtfProperty("managed")]
        public bool? Managed { get; private set; }

        /// <summary>
        /// whether this emoji is animated
        /// </summary>
        [EtfProperty("animated")]
        public bool? Animated { get; private set; }

        /// <summary>
        /// whether this emoji can be used, may be false due to loss of Server Boosts
        /// </summary>
        [EtfProperty("available")]
        public bool? Available { get; private set; }
    }
}
