using Integration.Likvido;
using Integration.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceReader.Application.Infrastructure
{
    public class CachedLikvidoClient : ILikvidoClient
    {
        private readonly ILikvidoClient _client;
        private readonly IDistributedCache _cache;

        public CachedLikvidoClient(ILikvidoClient client, IDistributedCache cache)
        {
            _client = client;
            _cache = cache;
        }
        public Task<Result> SendInvoiceAsync(string payload, CancellationToken cancellationToken = new())
        {
            return _client.SendInvoiceAsync(payload, cancellationToken);
        }

        public async Task<InvoiceResponse> GetInvoicesAsync()
        {
            var cachedJsonResult = await _cache.GetStringAsync(Constants.GetInvoicesKey);

            if (!string.IsNullOrEmpty(cachedJsonResult))
            {
                return JsonConvert.DeserializeObject<InvoiceResponse>(cachedJsonResult);
            }

            var clientResponse = await _client.GetInvoicesAsync();

            var jsonResult = JsonConvert.SerializeObject(clientResponse);
            await _cache.SetStringAsync(Constants.GetInvoicesKey, jsonResult);

            return clientResponse;
        }
    }
}
