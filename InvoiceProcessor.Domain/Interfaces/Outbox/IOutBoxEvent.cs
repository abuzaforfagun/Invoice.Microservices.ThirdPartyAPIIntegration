using System;

namespace InvoiceProcessor.Domain.Interfaces.Outbox
{
    public interface IOutBoxEvent
    {
        public Guid? MessageId { get; }
    }
}
