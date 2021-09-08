using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Integration.Models;
using Newtonsoft.Json;

namespace Integration.Likvido
{
    public record Result (bool IsSuccess, string? Message);

    public class LikvidoClient : ILikvidoClient
    {
        private readonly HttpClient _httpClient;
        private readonly LikvidoClientConfiguration _clientConfiguration;

        public LikvidoClient(HttpClient httpClient, LikvidoClientConfiguration clientConfiguration)
        {
            _httpClient = httpClient;
            _clientConfiguration = clientConfiguration;
        }

        public async Task<Result> SendInvoiceAsync(string payload, CancellationToken cancellationToken = new())
        {
            var requestBody = new StringContent(payload, Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(_clientConfiguration.SendInvoiceApiPath, requestBody, cancellationToken);

            return new Result(response.IsSuccessStatusCode, response.ReasonPhrase);
        }

        public async Task<InvoiceResponse> GetInvoicesAsync()
        {
            var jsonResult = await _httpClient.GetStringAsync(_clientConfiguration.GetInvoicesApiPath);
            return JsonConvert.DeserializeObject<Models.InvoiceResponse>(jsonResult);
        }
    }
}
