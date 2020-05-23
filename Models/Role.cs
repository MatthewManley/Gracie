using Gracie.ETF;

namespace Gracie.Models
{
    /// <summary>
    /// https://discord.com/developers/docs/topics/permissions#role-object
    /// </summary>
    public class Role
    {
        /// <summary>
        /// role id
        /// </summary>
        [EtfProperty("id")]
        public ulong Id { get; private set; }

        /// <summary>
        /// role name
        /// </summary>
        [EtfProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// integer representation of hexadecimal color code
        /// </summary>
        [EtfProperty("color")]
        public int Color { get; private set; }

        /// <summary>
        /// if this role is pinned in the user listing
        /// </summary>
        [EtfProperty("hoist")]
        public bool Hoist { get; private set; }

        /// <summary>
        /// position of this role
        /// </summary>
        [EtfProperty("position")]
        public int Position { get; private set; }

        /// <summary>
        /// permission bit set
        /// </summary>
        [EtfProperty("permissions")]
        public int Permisions { get; private set; }

        /// <summary>
        /// whether this role is managed by an integration
        /// </summary>
        [EtfProperty("managed")]
        public bool Managed { get; private set; }

        /// <summary>
        /// whether this role is mentionable
        /// </summary>
        [EtfProperty("mentionable")]
        public bool Mentionable { get; private set; }
    }
}
