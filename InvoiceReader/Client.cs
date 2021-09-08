using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceReader
{
    public interface IClient
    {
        Task<string> GetResultAsync();
    }
    public class Client : IClient
    {
        private readonly HttpClient _client;

        public Client(HttpClient client)
        {
            _client = client;
        }
        public Task<string> GetResultAsync()
        {
            return _client.GetStringAsync(
                "http://example.com");
        }
    }
    public class ClientController : Controller
    {
        private readonly IClient _client;

        public ClientController(IClient client)
        {
            _client = client;
        }

        public Task<string> Get()
        {
            return _client.GetResultAsync();
        }
    }
}
