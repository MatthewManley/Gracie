using Gracie.Gateway;
using Gracie.Gateway.Payload;
using Gracie.Gateway.Payload.Dispatch;
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();
            client = scope.ServiceProvider.GetRequiredService<GatewayClient>();
            client.HelloReceived += HelloRecievedSetupHeartbeat;
            client.HelloReceived += HelloRecievedSendIdentify;
            client.HeartbeatAckReceived += Client_HeartbeatAckRecieved;
            client.NewSequenceNumber += Client_NewSequenceNumber;
            client.ReadyReceived += Client_ReadyReceived1; ;
            client.TypingStartReceived += Client_TypingStartReceived;
            client.GuildCreateReceived += Client_GuildCreateReceived;
            var uri = new Uri("wss://gateway.discord.gg/?v=6&encoding=etf"); //TODO:3 this shouldn't be hardcoded, it should come from the rest api
            await client.Connect(uri);
            await client.Recieve();
        }

        private Task Client_GuildCreateReceived(object sender, GuildCreateEventPayload typingStartEventPayload)
        {
            return Task.CompletedTask;
        }

        private Task Client_TypingStartReceived(object sender, TypingStartEventPayload typingStartEventPayload)
        {
            return Task.CompletedTask;
        }

        public GatewayClient client;
        public int? lastSequenceNumber = null;
        private readonly ILogger<Startup> logger;

        private Task Client_ReadyReceived1(object sender, ReadyEventPayload readyPayload)
        {
            return Task.CompletedTask;
        }

        private Task Client_NewSequenceNumber(object sender, int sequenceNumber)
        {
            lastSequenceNumber = sequenceNumber;
            return Task.CompletedTask;
        }

        private Task Client_HeartbeatAckRecieved(object sender, HeartbeatAckPayload payload)
        {
            logger.LogInformation("Heartbeat ACK Recieved");
            return Task.CompletedTask;
        }

        private async Task HelloRecievedSendIdentify(object sender, HelloPayload payload)
        {
            logger.LogInformation("Sending heartbeat");
            await client.Send(new HeartbeatPayload(lastSequenceNumber));
            await client.Send(new IdentifyPayload
            {
                Token = Configuration["discordtoken"],
                Intents = Intent.GuildMessages | Intent.DirectMessages | Intent.GuildVoiceStates
            });
        }

        private Task HelloRecievedSetupHeartbeat(object sender, HelloPayload payload)
        {
            var heartbeat = Task.Run(async delegate
            {
                logger.LogInformation("Hello Recieved");
                while (true)
                {
                    await Task.Delay(payload.HeartbeatInterval);
                    logger.LogInformation("Sending heartbeat");
                    await client.Send(new HeartbeatPayload(lastSequenceNumber));
                }
            });
            return Task.CompletedTask;
        }
    }
}
