using System;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Events;

namespace InvoiceProcessor.Application.Commands.Outbox.Events
{
    public class InvoiceRequestProcessingStarted : OutBoxEvent
    {
        public InvoiceRequestProcessingStarted(Guid guid) : base(guid)
        {
            Status = OutBoxStatus.Processing;
        }
    }
}
