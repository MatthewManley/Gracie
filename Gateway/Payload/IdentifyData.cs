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
    public class IdentifyData
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


        public int? ShardId { get; set; } = null;
        public int? NumShards { get; set; } = null;

        [EtfProperty("shard")]
        public List<int> Shard
        {
            get
            {
                if (ShardId is null || NumShards is null)
                {
                    return null;
                }
                else
                {
                    return new List<int>() { ShardId.Value, NumShards.Value };
                }
            }
        }
        //TODO: presence
        //TODO: guild_subscriptions

        /// <summary>
        /// enables dispatching of guild subscription events (presence and typing events)	
        /// </summary>
        [EtfProperty("guild_subscriptions")]
        public bool? GuildSubscriptions { get; set; }

        /// <summary>
        /// the Gateway Intents you wish to receive
        /// </summary>
        [EtfProperty("intents")]
        public Intent? Intents { get; set; } = null;
    }
}
