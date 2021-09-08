using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication.Attributes;
using Communication.Messages;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Communication.Consumer
{
    public class DistributedConsumer : IDistributedConsumer
    {
        private QueueClient _queueClient;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public DistributedConsumer(ILogger<object> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        
        public void RegisterQueueHandler<T>(string connectionString, string queueName) where T:IRequest
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false,
            };

            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync<T>, messageHandlerOptions);
        }

        public async Task ProcessAsync<T>(T payload, IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            try
            {
                await mediator.Send(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var attributes = payload.GetType().GetAttributeValue((ServiceBusQueue att) => att);

                var isScheduleRequired = attributes.IsScheduleRequired;

                if (payload is IScheduledDistributedCommand scheduledCommand 
                    && isScheduleRequired 
                    && !scheduledCommand.IsDiffered)
                {

                    var queueName = attributes.Name;
                    var connectionString = serviceProvider.GetRequiredService<ServiceBusConfiguration>()
                        .PrimaryConnectionString;

                    scheduledCommand.Differ();

                    var queueClient = new QueueClient(connectionString, queueName);
                    
                    var payloadJson = JsonConvert.SerializeObject(scheduledCommand);
                    var message = new Message()
                    {
                        Body = Encoding.UTF8.GetBytes(payloadJson)
                    };
                    
                    var jitter = new Random();

                    await queueClient.ScheduleMessageAsync(message,
                        DateTimeOffset.UtcNow.AddMinutes(attributes.DifferedTimeInMinutes)
                            .AddMilliseconds(jitter.Next(0, 100)));
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task ProcessMessagesAsync<T>(Message message, CancellationToken token) where T:IRequest
        {
            using var serviceProviderScope = _serviceProvider.CreateScope();
            var payload = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body)) as IRequest;
            await ProcessAsync(payload, serviceProviderScope.ServiceProvider);
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
        
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            _logger.LogDebug($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }

        public async Task CloseQueueAsync()
        {
            await _queueClient.CloseAsync();
        }
    }
}
