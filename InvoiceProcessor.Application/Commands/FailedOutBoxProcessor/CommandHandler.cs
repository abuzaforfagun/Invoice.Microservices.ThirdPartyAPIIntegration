using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Application.Factories;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Messages;
using MediatR;

namespace InvoiceProcessor.Application.Commands.FailedOutBoxProcessor
{
    public class CommandHandler : AsyncRequestHandler<ProcessOutBoxFailedCommand>
    {
        private readonly IMediator _mediator;
        private readonly IOutBoxFactory _outBoxFactory;

        public CommandHandler(IMediator mediator, IOutBoxFactory outBoxFactory)
        {
            _mediator = mediator;
            _outBoxFactory = outBoxFactory;
        }
        protected override Task Handle(ProcessOutBoxFailedCommand request, CancellationToken cancellationToken)
        {
            var outBoxCommand = _outBoxFactory.GetOutBoxEvent(OutBoxStatus.Failed, guid: request.MessageId);
            return _mediator.Send(outBoxCommand, cancellationToken);
        }
    }
}
