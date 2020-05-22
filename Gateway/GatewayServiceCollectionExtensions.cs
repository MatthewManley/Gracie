using Gracie.ETF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;

namespace Gracie.Gateway
{
    public static class GatewayServiceCollectionExtensions
    {
        public static IServiceCollection AddGatewayServices(this IServiceCollection services) => services
                .AddTransient(x => new ObjectDeserializer(x.GetRequiredService<ILogger<ObjectDeserializer>>(), ETFConstants.Latin1))
                .AddTransient<ClientWebSocket>()
                .AddSingleton<GatewayClient>();
    }
}
