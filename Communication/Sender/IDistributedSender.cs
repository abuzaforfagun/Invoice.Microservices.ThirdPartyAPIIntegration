using System.Threading.Tasks;
using Communication.Messages;

namespace Communication.Sender
{
    public interface IDistributedSender
    {
        Task SendMessageAsync(IDistributedCommand payload);
        Task<T> GetAsync<T>(IDistributedQuery query);
    }
}
