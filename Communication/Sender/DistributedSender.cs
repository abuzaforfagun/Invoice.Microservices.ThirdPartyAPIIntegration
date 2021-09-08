using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Communication.Attributes;
using Communication.Messages;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace Communication.Sender
{
    public class DistributedSender : IDistributedSender
    {
        private readonly string _connectionString;
        private readonly HttpClient _httpClient;

        public DistributedSender(ServiceBusConfiguration config, HttpClient httpClient)
        {
            _connectionString = config.PrimaryConnectionString;
            _httpClient = httpClient;
        }

        public async Task SendMessageAsync(IDistributedCommand payload)
        {
            var attributes = payload.GetType().GetAttributeValue((ServiceBusQueue att) => att);

            var queueName = attributes.Name;
            
            await InitializeQueue(attributes.Name, attributes.DeadLetterReceiver, attributes.MaxRetry);

            var queueClient = new QueueClient(_connectionString, queueName);

            var data = JsonConvert.SerializeObject(payload);
            var message = new Message(Encoding.UTF8.GetBytes(data)) {MessageId = Guid.NewGuid().ToString()};
            await queueClient.SendAsync(message);
        }

        private async Task InitializeQueue(string queueName, string deadLetterReceiver = "", int maxRetry = 10)
        {
            var managementClient = new ManagementClient(_connectionString);
            if (!await managementClient.QueueExistsAsync(queueName))
            {
                var queueDescription = new QueueDescription(queueName)
                {
                    EnablePartitioning = false,
                    MaxSizeInMB = 1024,
                    RequiresDuplicateDetection = true,
                    MaxDeliveryCount = maxRetry
                };

                if (!string.IsNullOrEmpty(deadLetterReceiver))
                {
                    await InitializeQueue(deadLetterReceiver);
                    queueDescription.ForwardDeadLetteredMessagesTo = deadLetterReceiver;
                }

                await managementClient.CreateQueueAsync(queueDescription);
            }
        }

        public async Task<T> GetAsync<T>(IDistributedQuery query)
        {
            var path = query.GetType().GetAttributeValue((HttpEndPoint att) => att.Path);
            var requestBody = GetHttpQueryStringParameters(query);

            var url = $"{path}?{requestBody}";
            var result = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(result);
        }

        private string GetHttpQueryStringParameters<T>(T item)
        {
            var parameters = typeof(T)
                .GetProperties()
                .SelectMany(p =>
                {
                    var value = p.GetValue(item, null);
                    var values = p.PropertyType != typeof(string) && value is IEnumerable valuesCollection
                        ? valuesCollection.Cast<object>()
                        : new[] { value };

                    return values.Select(v =>
                        $"{p.Name}={HttpUtility.UrlEncode(Convert.ToString(v, CultureInfo.InvariantCulture))}");
                });

            return string.Join("&", parameters);
        }
        
    }
}
