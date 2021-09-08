using MediatR;

namespace InvoiceReader.Application.Commands.InvalidateCache
{
    public partial class InvalidateCache
    {
        public class Command : IRequest
        {
        }
    }
}
