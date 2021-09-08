using System;
using InvoiceProcessor.Application.Commands.Outbox;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using InvoiceProcessor.Domain.ValueObjects;

namespace InvoiceProcessor.Application.Factories
{
    public interface IOutBoxFactory
    {
        Outbox.Command<IOutBoxEvent> GetOutBoxEvent(OutBoxStatus status, IValueObject? data = null, Guid? guid = null);
    }
}
