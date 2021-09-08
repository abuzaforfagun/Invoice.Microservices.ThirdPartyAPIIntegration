using System;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.Outbox;

namespace InvoiceProcessor.Domain.Events
{
    public class OutBoxEvent : IOutBoxEvent
    {
        public OutBoxStatus Status { get; init; }
        public Guid? MessageId { get; }

        public OutBoxEvent(Guid? messageId)
        {
            MessageId = messageId;
        }
    }
}
