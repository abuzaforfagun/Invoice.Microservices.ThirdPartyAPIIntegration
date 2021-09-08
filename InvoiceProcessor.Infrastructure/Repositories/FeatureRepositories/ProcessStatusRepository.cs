using System;
using System.Threading.Tasks;
using InvoiceProcessor.Domain.Entities;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.GetProcessStatus;
using InvoiceProcessor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessor.Infrastructure.Repositories.FeatureRepositories
{
    public class ProcessStatusRepository : IProcessStatusRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProcessStatusRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OutBoxStatus> GetProcessStatus(Guid messageId)
        {
            var process = await _unitOfWork.Repository<OutboxItem>()
                                        .GetQuery()
                                        .SingleOrDefaultAsync(o => o.Guid == messageId);
            return process.Status;
        }
    }
}
