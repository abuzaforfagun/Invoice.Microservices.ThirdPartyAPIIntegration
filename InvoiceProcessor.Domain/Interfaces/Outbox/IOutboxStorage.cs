using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Domain.Entities;

namespace InvoiceProcessor.Domain.Interfaces.Outbox
{
    public interface IOutboxStorage
    {
        void AddItem(OutboxItem item);
        Task<List<OutboxItem>> GetPendingItemsAsync(CancellationToken cancellationToken = new());
        Task PersistPendingChanges(CancellationToken cancellationToken = new());
        Task Upsert(OutboxItem item, Guid? guid, CancellationToken cancellationToken = new());
    }
}
