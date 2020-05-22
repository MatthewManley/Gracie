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
    public class IdentifyPayload : SerializablePayload
    {
        //TODO: large_threshold, shard, presence, guild_subscriptions

        /// <summary>
        /// authentication token
        /// </summary>
        public string Token { get; set; }
        public static readonly string LibraryName = "Gracie v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public IdentifyPayload() : base(Opcode.Identify)
        {
        }

        public string OperatingSystem { get; set; } = Environment.OSVersion.VersionString;

        public Intent? Intents { get; set; } = null;

        public override bool HasData => true;

        public override int SerializeData(byte[] buffer, int position)
        {
            var s = SerializeItemHelpers.SerializeBinaryExt(LibraryName);
            var properties = new List<(string, ETFSerializer.SerializeItem)>
            {
                ("$os", SerializeItemHelpers.SerializeBinaryExt(OperatingSystem)),
                ("$browser", s),
                ("$device", s)
            };

            var items = new List<(string, ETFSerializer.SerializeItem)>
            {
                ("token", SerializeItemHelpers.SerializeBinaryExt(Token)),
                ("properties", SerializeItemHelpers.SerializeMapExt(properties))
            };

            if (Intents.HasValue)
            {
                items.Add(("intents", SerializeItemHelpers.SerializeIntegerExt((int)Intents)));
            }

            return ETFSerializer.SerializeMapExt(buffer, position, items);
        }
    }
}
