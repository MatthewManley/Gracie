using Gracie.ETF;
using Gracie.Gateway.Payload;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Gracie.Gateway.Payload
{
    /// <summary>
    /// https://discord.com/developers/docs/topics/gateway#identify-identify-structure
    /// </summary>
    public class IdentifyPayload
    {
        /// <summary>
        /// authentication token
        /// </summary>
        [EtfProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// connection properties
        /// </summary>
        [EtfProperty("properties")]
        public ConnectionProperties Properties { get; set; } = new ConnectionProperties();

        // TODO: compress

        /// <summary>
        /// value between 50 and 250, total number of members where the gateway will stop sending offline members in the guild member list
        /// </summary>
        [EtfProperty("large_threshold")]
        public int? LargeThreshold { get; set; }

        //TODO: shard
        //TODO: presence
        //TODO: guild_subscriptions

        /// <summary>
        /// the Gateway Intents you wish to receive
        /// </summary>
        [EtfProperty("intents")]
        public Intent? Intents { get; set; } = null;
    }
}
