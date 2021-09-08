using System.Threading.Tasks;
using MediatR;

namespace Communication.Consumer
{
    public interface IDistributedConsumer
    {
        void RegisterQueueHandler<T>(string connectionString, string queueName) where T : IRequest;
        Task CloseQueueAsync();
    }
}
