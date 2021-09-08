using System;
using InvoiceProcessor.Application.Commands.Outbox;
using InvoiceProcessor.Application.Commands.Outbox.Events;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using InvoiceProcessor.Domain.ValueObjects;
using InvoiceProcessor.Messages;

namespace InvoiceProcessor.Application.Factories
{
    public class OutBoxFactory : IOutBoxFactory
    {
        public Outbox.Command<IOutBoxEvent> GetOutBoxEvent(
            OutBoxStatus status, IValueObject? data = null, Guid? guid = null)
        {
            return status switch
            {
                OutBoxStatus.Pending => new Outbox.Command<IOutBoxEvent>(new InvoiceRequestCreated(data), typeof(SendInvoiceCommand).FullName),
                OutBoxStatus.Processing => new Outbox.Command<IOutBoxEvent>(new InvoiceRequestProcessingStarted(guid.Value)),
                OutBoxStatus.Processed => new Outbox.Command<IOutBoxEvent>(new InvoiceRequestCompleted(guid.Value)),
                OutBoxStatus.Failed => new Outbox.Command<IOutBoxEvent>(new InvoiceRequestFailed(guid.Value)),
                _ => null
            };
        }
    }
}
