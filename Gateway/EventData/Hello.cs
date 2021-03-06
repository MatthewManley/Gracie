﻿using Gracie.ETF;

namespace Gracie.Gateway.EventData
{
    /// <summary>
    /// Sent on connection to the websocket. Defines the heartbeat interval that the client should heartbeat to.
    /// https://discord.com/developers/docs/topics/gateway#hello
    /// </summary>
    public class Hello
    {
        /// <summary>
        /// the interval (in milliseconds) the client should heartbeat with
        /// </summary>
        [EtfProperty("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }
    }
}
