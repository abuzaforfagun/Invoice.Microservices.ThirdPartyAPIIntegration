using AutoMapper;
using InvoiceProcessor.Domain.Entities;

namespace InvoiceProcessor.Application.Queries.Outbox.GetPendingProcess
{
    public partial class GetPendingProcess
    {
        public class MapperConfiguration : Profile
        {
            public MapperConfiguration()
            {
                CreateMap<OutboxItem, Model>();
            }
        }
    }
}
