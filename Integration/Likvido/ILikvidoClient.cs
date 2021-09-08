using System.Threading;
using System.Threading.Tasks;
using Integration.Models;

namespace Integration.Likvido
{
    public interface ILikvidoClient
    {
        Task<Result> SendInvoiceAsync(string payload, CancellationToken cancellationToken = new());
        Task<InvoiceResponse> GetInvoicesAsync();
    }
}
