using InvoiceProcessor.Domain.ValueObjects;
using MediatR;

namespace InvoiceProcessor.Application.Commands.CreateInvoiceRequest
{
    public partial class CreateInvoiceRequest
    {
        public class Command : InvoiceRequest, IRequest
        {
        }
    }
}
