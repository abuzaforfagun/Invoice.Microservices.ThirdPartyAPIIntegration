using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Infrastructure.Outbox;

namespace InvoiceProcessor.Domain.Interfaces.Outbox
{
    public interface IOutBoxService
    {
        Task Upsert(OutBoxModel model, CancellationToken cancellationToken = new());
    }
}