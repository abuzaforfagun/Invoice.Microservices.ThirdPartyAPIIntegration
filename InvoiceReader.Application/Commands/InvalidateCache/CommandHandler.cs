using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace InvoiceReader.Application.Commands.InvalidateCache
{
    public partial class InvalidateCache
    {
        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly IDistributedCache _cache;

            public CommandHandler(IDistributedCache cache)
            {
                _cache = cache;
            }
            protected override Task Handle(Command request, CancellationToken cancellationToken)
            {
                return _cache.RemoveAsync(Constants.GetInvoicesKey, cancellationToken);
            }
        }
    }
}
