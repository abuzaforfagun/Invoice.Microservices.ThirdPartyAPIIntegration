using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Domain.Interfaces.GetProcessStatus;
using InvoiceProcessor.Messages;
using MediatR;

namespace InvoiceProcessor.Application.Queries.Outbox.GetProcessStatus
{
    public partial class GetProcessStatus
    {
        public class QueryHandler : IRequestHandler<GetProcessStatusQuery, GetProcessStatusResult>
        {
            private readonly IProcessStatusRepository _repository;

            public QueryHandler(IProcessStatusRepository repository)
            {
                _repository = repository;
            }

            public async Task<GetProcessStatusResult> Handle(
                GetProcessStatusQuery request, CancellationToken cancellationToken)
            {
                var processStatus = await _repository.GetProcessStatus(request.MessageId);
                return new GetProcessStatusResult(processStatus);
            }
        }
    }
}
