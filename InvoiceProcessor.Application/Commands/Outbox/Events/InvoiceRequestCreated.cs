using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Events;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using InvoiceProcessor.Domain.ValueObjects;

namespace InvoiceProcessor.Application.Commands.Outbox.Events
{
    public class InvoiceRequestCreated : OutBoxEvent, IOutBoxValueEvent
    {
        public IValueObject Data { get; }

        public InvoiceRequestCreated(IValueObject data) : base(null)
        {
            Data = data;
            Status = OutBoxStatus.Pending;
        }
    }
}
