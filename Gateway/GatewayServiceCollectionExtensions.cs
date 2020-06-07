using Gracie.ETF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Threading;

namespace Gracie.Gateway
{
    public static class GatewayServiceCollectionExtensions
    {
        public static IServiceCollection AddGatewayServices(this IServiceCollection services) => services
                .AddTransient(x => new ObjectDeserializer(x.GetRequiredService<ILogger<ObjectDeserializer>>(), ETFConstants.Latin1))
                .AddTransient<ClientWebSocket>()
                .AddSingleton(new SemaphoreSlim(1))
                .AddSingleton(x =>
                {
                    var webSocket = x.GetRequiredService<ClientWebSocket>();
                    var semaphoreSlim = x.GetRequiredService<SemaphoreSlim>();
                    var logger = x.GetRequiredService<ILogger<GatewayClient>>();
                    var objectDeserializer = x.GetRequiredService<ObjectDeserializer>();
                    return new GatewayClient(webSocket, semaphoreSlim, 1024 * 1024, logger, objectDeserializer);
                });
    }
}
