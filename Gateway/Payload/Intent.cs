using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Gateway.Payload
{
    /// <summary>
    /// https://discord.com/developers/docs/topics/gateway#gateway-intents
    /// </summary>
    [Flags]
    public enum Intent : int
    {
        Guilds = 1 << 0,
        GuildMembers = 1 << 1,
        GuildBans = 1 << 2,
        GuildEmojis = 1 << 3,
        GuildIntegrations = 1 << 4,
        GuildWebHooks = 1 << 5,
        GuildInvites = 1 << 6,
        GuildVoiceStates = 1 << 7,
        GuildPresences = 1 << 8,
        GuildMessages = 1 << 9,
        GuildMessageReactions = 1 << 10,
        GuildMessageTyping = 1 << 11,
        DirectMessages = 1 << 12,
        DirectMessageReactions = 1 << 13,
        DirectMessageTyping = 1 << 14
    }
}
