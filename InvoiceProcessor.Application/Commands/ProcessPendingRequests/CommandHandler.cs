using System;
using System.Reflection;
using InvoiceProcessor.Application.Queries.Outbox.GetPendingProcess;
using InvoiceProcessor.Infrastructure.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Communication.Messages;
using Communication.Sender;
using InvoiceProcessor.Application.Factories;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Messages;
using Newtonsoft.Json;

namespace InvoiceProcessor.Application.Commands.ProcessPendingRequests
{
    public partial class ProcessPendingRequests
    {
        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly IMediator _mediator;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IDistributedSender _distributedSender;
            private readonly IOutBoxFactory _outBoxFactory;

            public CommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IDistributedSender distributedSender, IOutBoxFactory outBoxFactory)
            {
                _mediator = mediator;
                _unitOfWork = unitOfWork;
                _distributedSender = distributedSender;
                _outBoxFactory = outBoxFactory;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var pendingProcesses = await _mediator.Send(new GetPendingProcess.Query(), cancellationToken);

                foreach (var process in pendingProcesses)
                {
                    await _unitOfWork.RunInTransactionAsync(async () =>
                      {
                          var outBoxCommand = _outBoxFactory.GetOutBoxEvent(OutBoxStatus.Processing, guid: process.Guid);

                          await _mediator.Send(outBoxCommand, cancellationToken);

                          var assembly = typeof(SendInvoiceCommand).Assembly;
                          var type = assembly.GetType(process.CommandType);

                          var command = (IDistributedCommand)JsonConvert.DeserializeObject(process.Data, type, new JsonSerializerSettings
                          {
                              MissingMemberHandling = MissingMemberHandling.Error
                          });
                          command.SetMessageId(process.Guid);

                          await _distributedSender.SendMessageAsync(command);
                      }, cancellationToken);
                }
            }
        }
    }
}
