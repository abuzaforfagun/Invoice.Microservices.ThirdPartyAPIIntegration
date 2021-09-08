using System;
using System.Threading.Tasks;
using InvoiceProcessor.Domain.Enums;

namespace InvoiceProcessor.Domain.Interfaces.GetProcessStatus
{
    public interface IProcessStatusRepository
    {
        Task<OutBoxStatus> GetProcessStatus(Guid messageId);
    }
}
