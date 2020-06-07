using Gracie.ETF;

namespace Gracie.Gateway.EventData
{
    /// <summary>
    /// https://discord.com/developers/docs/topics/gateway#client-status-object
    /// </summary>
    public class ClientStatus
    {
        /// <summary>
        /// the user's status set for an active desktop (Windows, Linux, Mac) application session
        /// </summary>
        [EtfProperty("desktop")]
        public string Desktop { get; set; }

        /// <summary>
        /// the user's status set for an active mobile (iOS, Android) application session
        /// </summary>
        [EtfProperty("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// the user's status set for an active web (browser, bot account) application session
        /// </summary>
        [EtfProperty("web")]
        public string Web { get; set; }
    }
}
