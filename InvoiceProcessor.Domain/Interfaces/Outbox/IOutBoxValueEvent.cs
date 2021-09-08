using InvoiceProcessor.Domain.ValueObjects;

namespace InvoiceProcessor.Domain.Interfaces.Outbox
{
    public interface IOutBoxValueEvent : IOutBoxEvent
    {
        public IValueObject Data { get; }
    }
}
