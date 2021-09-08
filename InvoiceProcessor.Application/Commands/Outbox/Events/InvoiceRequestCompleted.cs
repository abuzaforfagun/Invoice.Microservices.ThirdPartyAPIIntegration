using System;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Events;

namespace InvoiceProcessor.Application.Commands.Outbox.Events
{
    public class InvoiceRequestCompleted : OutBoxEvent
    {
        public InvoiceRequestCompleted(Guid guid):base(guid)
        {
            Status = OutBoxStatus.Processed;
        }
    }
}
