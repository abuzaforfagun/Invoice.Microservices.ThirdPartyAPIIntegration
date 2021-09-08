using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Integration.Likvido;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace InvoiceReader.Application.Queries.GetInvoices
{
    public partial class GetInvoices
    {
        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly ILikvidoClient _likvidoClient;
            private readonly IMapper _mapper;
            private readonly IDistributedCache _cache;

            public QueryHandler(ILikvidoClient likvidoClient, IMapper mapper, IDistributedCache cache)
            {
                _likvidoClient = likvidoClient;
                _mapper = mapper;
                _cache = cache;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var clientResponse = await _likvidoClient.GetInvoicesAsync();
                var result = _mapper.Map<Result>(clientResponse);
                
                return result;
            }
        }
    }
}
