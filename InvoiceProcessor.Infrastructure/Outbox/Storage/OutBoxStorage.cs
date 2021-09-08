using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Domain.Entities;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using InvoiceProcessor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessor.Infrastructure.Outbox.Storage
{
    public class OutBoxStorage : IOutboxStorage
    {
        private readonly IUnitOfWork _unitOfWork;

        public OutBoxStorage(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddItem(OutboxItem item)
        {
            _unitOfWork.Repository<OutboxItem>().Add(item);
        }

        public Task<List<OutboxItem>> GetPendingItemsAsync(CancellationToken cancellationToken = new()) =>
            _unitOfWork.Repository<OutboxItem>().GetQuery().Where(i => i.Status == OutBoxStatus.Pending)
                .ToListAsync(cancellationToken);

        public Task PersistPendingChanges(CancellationToken cancellationToken = new())
        {
            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task Upsert(OutboxItem item, Guid? guid, CancellationToken cancellationToken = new())
        {
            if (guid is null)
            {
                AddItem(item);
            }
            else
            {
                var existingProcess = await _unitOfWork.Repository<OutboxItem>().GetQuery().SingleAsync(o => o.Guid == guid, cancellationToken);
                existingProcess.UpdateStatus(item.Status);
            }

            await PersistPendingChanges(cancellationToken);
        }
    }
}
