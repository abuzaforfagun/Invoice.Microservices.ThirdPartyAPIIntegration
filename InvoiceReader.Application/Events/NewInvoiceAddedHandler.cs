using System;
using System.Threading;
using System.Threading.Tasks;
using Communication.Sender;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Messages;
using InvoiceReader.Application.Commands.InvalidateCache;
using MediatR;
using InvoiceReader.Messages;

namespace InvoiceReader.Application.Events
{
    public class NewInvoiceAddedHandler : AsyncRequestHandler<NewInvoiceAdded>
    {
        private readonly IMediator _mediator;
        private readonly IDistributedSender _distributedSender;

        public NewInvoiceAddedHandler(IMediator mediator, IDistributedSender distributedSender)
        {
            _mediator = mediator;
            _distributedSender = distributedSender;
        }

        protected override async Task Handle(NewInvoiceAdded request, CancellationToken cancellationToken)
        {
            var processStatus =
                await _distributedSender.GetAsync<GetProcessStatusResult>(
                    new GetProcessStatusQuery(request.MessageId));
            
            if(processStatus.Status == OutBoxStatus.Failed)
            {
                return;
            }

            if (processStatus.Status != OutBoxStatus.Processed)
            {
                throw new Exception($"Processing of {request.MessageId} not yet completed");
            }
            await _mediator.Send(new InvalidateCache.Command(), cancellationToken);
        }
    }
}
