using Gracie.ETF;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gracie.Gateway.Payload
{
    public class ConnectionProperties
    {
        public static readonly string LibName = "Gracie v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [EtfProperty("$os")]
        public string OperatingSystem { get; set; } = Environment.OSVersion.VersionString;

        [EtfProperty("$browser")]
        public string Browser { get; set; } = LibName;

        [EtfProperty("$device")]
        public string Device { get; set; } = LibName;
    }
}
