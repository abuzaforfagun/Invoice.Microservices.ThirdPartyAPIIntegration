using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using MediatR;

namespace InvoiceProcessor.Application.Queries.Outbox.GetPendingProcess
{
    public partial class GetPendingProcess
    {
        public class QueryHandler : IRequestHandler<Query, List<Model>>
        {
            private readonly IOutboxStorage _outboxStorage;
            private readonly IMapper _mapper;

            public QueryHandler(IOutboxStorage outboxStorage, IMapper mapper)
            {
                _outboxStorage = outboxStorage;
                _mapper = mapper;
            }

            public async Task<List<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pendingItems = await _outboxStorage.GetPendingItemsAsync(cancellationToken);
                
                return _mapper.Map<List<Model>>(pendingItems);
            }
        }
    }
}
