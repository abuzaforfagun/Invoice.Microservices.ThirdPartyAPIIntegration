using MediatR;
using System.Collections.Generic;

namespace InvoiceProcessor.Application.Queries.Outbox.GetPendingProcess
{
    public partial class GetPendingProcess
    {
        public class Query : IRequest<List<Model>>
        {
        }
    }
}
