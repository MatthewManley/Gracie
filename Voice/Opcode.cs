using System;
using System.Collections.Generic;
using System.Text;

namespace Voice
{
    /// <summary>
    /// https://discord.com/developers/docs/topics/opcodes-and-status-codes#voice-voice-opcodes
    /// </summary>
    public enum Opcode : byte
    {
        /// <summary>
        /// Sent by client
        /// Begin a voice websocket connection
        /// </summary>
        Identify = 0,

        /// <summary>
        /// Sent by client
        /// Select the voice protocol
        /// </summary>
        SelectProtocol = 1,

        /// <summary>
        /// Sent by server
        /// Complete the websocket handshake
        /// </summary>
        Ready = 2,

        /// <summary>
        /// Sent by client
        /// Keep the websocket connection alive.
        /// </summary>
        Heartbeat = 3,

        /// <summary>
        /// Sent by server
        /// Describe the session.
        /// </summary>
        SessionDescription = 4,

        /// <summary>
        /// Sent by client and server
        /// Indicate which users are speaking.
        /// </summary>
        Speaking = 5,

        /// <summary>
        /// Sent by server
        /// Sent to acknowledge a received client heartbeat.
        /// </summary>
        HeartbeatAck = 6,

        /// <summary>
        /// Sent by client
        /// Resume a connection.
        /// </summary>
        Resume = 7,

        /// <summary>
        /// Sent by server
        /// Time to wait between sending heartbeats in milliseconds.
        /// </summary>
        Hello = 8,

        /// <summary>
        /// Sent by server
        /// Acknowledge a successful session resume.
        /// </summary>
        Resumed = 9,

        /// <summary>
        /// Sent by server
        /// A client has disconnected from the voice channel
        /// </summary>
        ClientDisconnect = 10
    }
}
