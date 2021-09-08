using MediatR;

namespace InvoiceReader.Application.Queries.GetInvoices
{
    public partial class GetInvoices
    {
        public record Query : IRequest<Result>
        {
        }
    }
}
