using InvoiceProcessor.Domain.Interfaces.Outbox;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceProcessor.Application.Queries.Outbox.GetPendingProcess
{
    public partial class GetPendingProcess
    {
        public class QueryHandler : IRequestHandler<Query, List<Model>>
        {
            private readonly IOutboxStorage _outboxStorage;

            public QueryHandler(IOutboxStorage outboxStorage)
            {
                _outboxStorage = outboxStorage;
            }

            public async Task<List<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pendingItems = await _outboxStorage.GetPendingItemsAsync(cancellationToken);

                return pendingItems?.Select(x => new Model(
                    guid: x.Guid.Value,
                    commandType: x.CommandType,
                    data: x.Data))
                    .ToList();
            }
        }
    }
}
