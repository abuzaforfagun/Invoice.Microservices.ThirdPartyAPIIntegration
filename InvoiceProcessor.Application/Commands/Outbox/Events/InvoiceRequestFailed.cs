using System;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Events;

namespace InvoiceProcessor.Application.Commands.Outbox.Events
{
    public class InvoiceRequestFailed : OutBoxEvent
    {
        public InvoiceRequestFailed(Guid guid) : base(guid)
        {
            Status = OutBoxStatus.Failed;
        }
    }
}
