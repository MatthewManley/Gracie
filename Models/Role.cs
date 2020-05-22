using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

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
        [PropertyName("id")]
        public BigInteger Id { get; private set; }

        /// <summary>
        /// role name
        /// </summary>
        [PropertyName("name")]
        public string Name { get; private set; }

        /// <summary>
        /// integer representation of hexadecimal color code
        /// </summary>
        [PropertyName("color")]
        public int Color { get; private set; }

        /// <summary>
        /// if this role is pinned in the user listing
        /// </summary>
        [PropertyName("hoist")]
        public bool Hoist { get; private set; }

        /// <summary>
        /// position of this role
        /// </summary>
        [PropertyName("position")]
        public int Position { get; private set; }

        /// <summary>
        /// permission bit set
        /// </summary>
        [PropertyName("permissions")]
        public int Permisions { get; private set; }

        /// <summary>
        /// whether this role is managed by an integration
        /// </summary>
        [PropertyName("managed")]
        public bool Managed { get; private set; }

        /// <summary>
        /// whether this role is mentionable
        /// </summary>
        [PropertyName("mentionable")]
        public bool Mentionable { get; private set; }
    }
}
