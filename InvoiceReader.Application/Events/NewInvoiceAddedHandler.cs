using System.Threading;
using System.Threading.Tasks;
using InvoiceReader.Application.Commands.InvalidateCache;
using MediatR;
using InvoiceReader.Messages;

namespace InvoiceReader.Application.Events
{
    public class NewInvoiceAddedHandler : AsyncRequestHandler<NewInvoiceAdded>
    {
        private readonly IMediator _mediator;

        public NewInvoiceAddedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override Task Handle(NewInvoiceAdded request, CancellationToken cancellationToken)
            => _mediator.Send(new InvalidateCache.Command(), cancellationToken);
    }
}
