using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gracie.Http
{
    public class GatewayApi
    {
        private readonly IOptions<HttpApiOptions> options;
        private readonly HttpApiClient apiClient;

        public GatewayApi(IOptions<HttpApiOptions> options, HttpApiClient apiClient)
        {
            this.options = options;
            this.apiClient = apiClient;
        }

        public async Task GetGateway(CancellationToken cancellationToken = default)
        {
            var method = HttpMethod.Get;
            var path = "/gateway";
            var uri = HttpApiClient.BuildUri(options.Value.BaseUrl, path);
            var request = new HttpRequestMessage(method, uri);
            var authHash = apiClient.GetAuthHash();
            var methodBytes = Encoding.ASCII.GetBytes(method.Method);
            var pathBytes = Encoding.ASCII.GetBytes(path);
            var operation = authHash.Concat(methodBytes).Concat(pathBytes).ToArray();

            await apiClient.SendRequest(operation, request, cancellationToken);
        }
    }
}
