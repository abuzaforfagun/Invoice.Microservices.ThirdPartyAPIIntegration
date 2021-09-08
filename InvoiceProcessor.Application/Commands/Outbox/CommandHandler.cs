using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using InvoiceProcessor.Infrastructure.Outbox;
using MediatR;

namespace InvoiceProcessor.Application.Commands.Outbox
{
    public partial class Outbox
    {
        public class CommandHandler : AsyncRequestHandler<Command<IOutBoxEvent>>
        {
            private readonly IOutBoxService _outBoxService;

            public CommandHandler(IOutBoxService outBoxService)
            {
                _outBoxService = outBoxService;
            }

            protected override Task Handle(Command<IOutBoxEvent> request, CancellationToken cancellationToken)
            {
                var messageId = request.Status == OutBoxStatus.Pending ? null : request.Data.MessageId;
                var model = new OutBoxModel(request.Data, request.Status, request.CommandName, messageId);

                return _outBoxService.Upsert(model, cancellationToken);
            }
        }
    }
}
