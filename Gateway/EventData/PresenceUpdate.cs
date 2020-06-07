using Gracie.ETF;
using Gracie.Models;
using System;
using System.Collections.Generic;

namespace Gracie.Gateway.EventData
{
    /// <summary>
    /// https://discord.com/developers/docs/topics/gateway#presence-update
    /// </summary>
    public class PresenceUpdate
    {
        /// <summary>
        /// the user presence is being updated for
        /// </summary>
        [EtfProperty("user")]
        public User User { get; set; }

        /// <summary>
        /// roles this user is in
        /// </summary>
        [EtfProperty("roles")]
        public List<ulong> Roles { get; set; }

        //TODO: game (Activity object) https://discord.com/developers/docs/topics/gateway#activity-object

        /// <summary>
        /// id of the guild
        /// </summary>
        [EtfProperty("guild_id")]
        public ulong GuildId { get; set; }

        /// <summary>
        /// either "idle", "dnd", "online", or "offline"
        /// </summary>
        [EtfProperty("status")]
        public string Status { get; set; }

        //TODO: activities (array of activity objects)

        /// <summary>
        /// user's platform-dependent status
        /// </summary>
        [EtfProperty("client_status")]
        public ClientStatus ClientStatus { get; set; }

        /// <summary>
        /// when the user started boosting the guild
        /// </summary>
        [EtfProperty("premium_since")]
        public DateTime? PremiumSince { get; set; }

        /// <summary>
        /// this users guild nickname (if one is set)
        /// </summary>
        [EtfProperty("nick")]
        public string Nick { get; set; }
    }
}
