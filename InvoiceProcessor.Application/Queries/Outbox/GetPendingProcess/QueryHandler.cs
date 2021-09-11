using InvoiceProcessor.Domain.Entities;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using MediatR;
using System.Collections.Generic;
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
                var result = PopulateData(pendingItems);

                return result;
            }

            private List<Model> PopulateData(List<OutboxItem> outboxItems)
            {
                var result = new List<Model>();
                foreach (var outboxItem in outboxItems)
                {
                    var model = new Model
                    (
                        guid: outboxItem.Guid.Value,
                        commandType: outboxItem.CommandType,
                        data: outboxItem.Data
                    );

                    result.Add(model);
                }

                return result;
            }
        }
    }
}
