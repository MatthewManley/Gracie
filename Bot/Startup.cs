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
            client.HelloReceived += HelloRecievedSendIdentify;
            client.HelloReceived += HelloRecievedSetupHeartbeat;
            client.HeartbeatAckReceived += Client_HeartbeatAckRecieved;
            client.NewSequenceNumber += Client_NewSequenceNumber;
            client.ReadyReceived += Client_ReadyReceived;
            client.TypingStartReceived += Client_TypingStartReceived1;
            client.MessageCreateReceived += Client_MessageCreateReceived;
            var uri = new Uri("wss://gateway.discord.gg/?v=6&encoding=etf"); //TODO: this shouldn't be hardcoded, it should come from the rest api
            await client.Connect(uri);
            await client.Recieve();
        }

        private Task Client_TypingStartReceived1(object sender, DataPayload<TypingStartEventData> typingStartEventPayload)
        {
            throw new NotImplementedException();
        }

        private Task Client_ReadyReceived(object sender, DataPayload<ReadyEventData> readyPayload)
        {
            logger.LogInformation("Ready Recieved");
            return Task.CompletedTask;
        }

        private Task Client_MessageCreateReceived(object sender, DataPayload<Gracie.Models.Message> messageCreatePayload)
        {
            logger.Log(LogLevel.Information, messageCreatePayload.Data.Content);
            return Task.CompletedTask;
        }

        public GatewayClient client;
        public int? lastSequenceNumber = null;
        private readonly ILogger<Startup> logger;

        private Task Client_NewSequenceNumber(object sender, int sequenceNumber)
        {
            lastSequenceNumber = sequenceNumber;
            return Task.CompletedTask;
        }

        private Task Client_HeartbeatAckRecieved(object sender, Payload payload)
        {
            logger.LogInformation("Heartbeat ACK Recieved");
            return Task.CompletedTask;
        }

        private async Task HelloRecievedSendIdentify(object sender, DataPayload<HelloData> payload)
        {
            logger.LogInformation("Hello Recieved");
            await SendHeartbeat();
            await client.Send(new DataPayload<IdentifyData>
            {
                Data = new IdentifyData
                {
                    GuildSubscriptions = false,
                    Intents = Intent.GuildMessages | Intent.DirectMessages,
                    Token = Configuration["discordtoken"]
                },
                Opcode = Opcode.Identify
            });
        }

        private async Task SendHeartbeat()
        {
            logger.LogInformation("Sending heartbeat");
            await client.Send(new DataPayload<int?>
            {
                Data = lastSequenceNumber,
                Opcode = Opcode.Heartbeat,
            });
        }

        private Task HelloRecievedSetupHeartbeat(object sender, DataPayload<HelloData> payload)
        {
            var heartbeat = Task.Run(async delegate
            {
                while (true)
                {
                    await Task.Delay(payload.Data.HeartbeatInterval);
                    await SendHeartbeat();
                }
            });
            return Task.CompletedTask;
        }
    }
}
