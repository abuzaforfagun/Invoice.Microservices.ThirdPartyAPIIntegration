using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Application.Factories;
using InvoiceProcessor.Domain.Enums;
using MediatR;

namespace InvoiceProcessor.Application.Commands.CreateInvoiceRequest
{
    public partial class CreateInvoiceRequest
    {
        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly IMediator _mediator;
            private readonly IOutBoxFactory _outBoxFactory;

            public CommandHandler(IMediator mediator, IOutBoxFactory outBoxFactory)
            {
                _mediator = mediator;
                _outBoxFactory = outBoxFactory;
            }

            protected override Task Handle(Command request, CancellationToken cancellationToken)
            {
                var outBoxCommand = _outBoxFactory.GetOutBoxEvent(OutBoxStatus.Pending, request);

                return _mediator.Send(outBoxCommand, cancellationToken);
            }
        }
    }
}
