using System;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.Outbox;

namespace InvoiceProcessor.Infrastructure.Outbox
{
    public record OutBoxModel(IOutBoxEvent Payload, OutBoxStatus Status, string CommandName, Guid? Guid = null);
}
