using Integration.Likvido;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceReader.Application.Queries.GetInvoices
{
    public partial class GetInvoices
    {
        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly ILikvidoClient _likvidoClient;
            private readonly IDistributedCache _cache;

            public QueryHandler(ILikvidoClient likvidoClient, IDistributedCache cache)
            {
                _likvidoClient = likvidoClient;
                _cache = cache;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var clientResponse = await _likvidoClient.GetInvoicesAsync();
                var result = new Result(clientResponse.Data);

                return result;
            }
        }
    }
}
