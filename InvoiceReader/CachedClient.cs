using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace InvoiceReader
{
    public class CachedClient : IClient
    {
        private readonly IDistributedCache _cache;
        private readonly IClient _client;
        public CachedClient(IDistributedCache cache, 
            IClient client)
        {
            _cache = cache;
            _client = client;
        }
        public async Task<string> GetResultAsync()
        {
            var cachedResult = await 
                _cache.GetStringAsync("KEY");

            if (!string.IsNullOrEmpty(cachedResult))
            {
                return cachedResult;
            }
            var result = await _client
                .GetResultAsync();

            await _cache.SetStringAsync("KEY", result);
            return result;
        }
    }
}