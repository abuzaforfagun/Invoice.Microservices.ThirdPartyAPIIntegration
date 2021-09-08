using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Events;
using MediatR;

namespace InvoiceProcessor.Application.Commands.Outbox
{
    public partial class Outbox
    {
        public class Command<IOutBoxEvent> : IRequest
        {
            public IOutBoxEvent Data { get; }
            public OutBoxStatus Status { get; }
            public string CommandName { get; }
            
            public Command(IOutBoxEvent data, string commandName = "")
            {
                Data = data;
                Status = (data as OutBoxEvent).Status;
                CommandName = commandName;
            }
        }
    }
}
