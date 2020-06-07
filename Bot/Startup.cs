using Gracie.Core;
using Gracie.Gateway;
using Gracie.Gateway.EventData;
using Gracie.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Bot
{
    public class Startup : BackgroundService
    {
        public Startup(IConfiguration configuration, IServiceProvider services, IHostEnvironment environment, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Services = services;
            Environment = environment;
            this.logger = logger;
        }

        public IConfiguration Configuration { get; }
        public IServiceProvider Services { get; }
        public IHostEnvironment Environment { get; }
        private const int MaxSendPayload = 4096;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();
            client = scope.ServiceProvider.GetRequiredService<GatewayClient>();
            client.HelloReceived += HelloRecievedSendIdentify;
            client.HelloReceived += HelloRecievedSetupHeartbeat;
            client.HeartbeatAckReceived += Client_HeartbeatAckRecieved;
            client.NewSequenceNumber += Client_NewSequenceNumber;
            client.ReadyReceived += Client_ReadyReceived;
            client.TypingStartReceived += Client_TypingStartReceived1;
            client.MessageCreateReceived += Client_MessageCreateReceived;
            client.VoiceServerUpdateReceived += Client_VoiceServerUpdateReceived;
            var uri = new Uri("wss://gateway.discord.gg/?v=6&encoding=etf"); //TODO: this shouldn't be hardcoded, it should come from the rest api
            await client.ConnectAsync(uri);
            await client.Recieve();
        }

        private Task Client_VoiceServerUpdateReceived(object sender, GatewayDataPayload<VoiceServerUpdate> voiceServerUpdatePayload, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private Task Client_TypingStartReceived1(object sender, DataPayload<TypingStart> typingStartEventPayload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task Client_ReadyReceived(object sender, DataPayload<Ready> readyPayload, CancellationToken cancellationToken)
        {
            logger.LogInformation("Ready Recieved");
            return Task.CompletedTask;
        }

        private async Task Client_MessageCreateReceived(object sender, DataPayload<Gracie.Models.Message> messageCreatePayload, CancellationToken cancellationToken)
        {
            //logger.Log(LogLevel.Information, messageCreatePayload.Data.Content);
            if (ulong.TryParse(messageCreatePayload.Data.Content, out var result))
            {
                await SendVoiceServerUpdate(messageCreatePayload.Data.GuildId.Value, result, cancellationToken);
            }
            var content = Convert.ToUInt64(messageCreatePayload.Data.Content);
        }

        public GatewayClient client;
        public int? lastSequenceNumber = null;
        private readonly ILogger<Startup> logger;

        private Task Client_NewSequenceNumber(object sender, int sequenceNumber, CancellationToken cancellation)
        {
            lastSequenceNumber = sequenceNumber;
            return Task.CompletedTask;
        }

        private Task Client_HeartbeatAckRecieved(object sender, Payload payload, CancellationToken cancellationToken)
        {
            logger.LogInformation("Heartbeat ACK Recieved");
            return Task.CompletedTask;
        }

        private async Task HelloRecievedSendIdentify(object sender, DataPayload<Hello> payload, CancellationToken cancellationToken)
        {
            logger.LogInformation("Hello Recieved");
            await SendHeartbeat(cancellationToken);
            var sendPayload = new GatewayDataPayload<Identify>
            {
                Data = new Identify
                {
                    GuildSubscriptions = false,
                    Intents = Intent.GuildMessages | Intent.DirectMessages | Intent.GuildVoiceStates,
                    Token = Configuration["discordtoken"]
                },
                GatewayOpcode = Opcode.Identify
            };
            await client.SendPayload(sendPayload, MaxSendPayload, cancellationToken);
        }

        private async Task SendHeartbeat(CancellationToken cancellationToken)
        {
            logger.LogInformation("Sending heartbeat");
            var dataPayload = new GatewayDataPayload<int?>
            {
                Data = lastSequenceNumber,
                GatewayOpcode = Opcode.Heartbeat,
            };
            await client.SendPayload(dataPayload, MaxSendPayload, cancellationToken);
        }

        private Task HelloRecievedSetupHeartbeat(object sender, DataPayload<Hello> payload, CancellationToken cancellationToken)
        {
            var heartbeat = Task.Run(async delegate
            {
                while (true)
                {
                    await Task.Delay(payload.Data.HeartbeatInterval);
                    await SendHeartbeat(cancellationToken);
                }
            });
            return Task.CompletedTask;
        }

        private async Task SendVoiceServerUpdate(ulong guildId, ulong channelId, CancellationToken cancellationToken)
        {
            var sendPayload = new GatewayDataPayload<VoiceState>
            {
                GatewayOpcode = Opcode.VoiceStateUpdate,
                Data = new VoiceState
                {
                    GuildId = guildId,
                    ChannelId = channelId,
                    SelfMute = false,
                    SelfDeaf = false,
                }
            };
            await client.SendPayload(sendPayload, MaxSendPayload, cancellationToken);
        }
    }
}
