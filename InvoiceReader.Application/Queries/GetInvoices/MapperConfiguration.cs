using AutoMapper;
using Integration.Models;
using InvoiceReader.Domain.Entity;

namespace InvoiceReader.Application.Queries.GetInvoices
{
    public partial class GetInvoices
    {
        public class MapperConfiguration : Profile
        {
            public MapperConfiguration()
            {
                CreateMap<InvoiceResponse, Result>();

                CreateMap<InvoiceItem, Invoice>(MemberList.Destination)
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                    .ForMember(d => d.CreatedOn, opt => opt.MapFrom(s => s.Attributes.DateCreated))
                    .ForMember(d => d.CreditorReference, opt => opt.MapFrom(s => s.Attributes.CreditorReference))
                    .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Attributes.Currency))
                    .ForMember(d => d.DueDate, opt => opt.MapFrom(s => s.Attributes.DueDate))
                    .ForMember(d => d.ExpireDate, opt => opt.MapFrom(s => s.Attributes.InvoiceExpirationDate))
                    .ForMember(d => d.Gross, opt => opt.MapFrom(s => s.Attributes.GrossAmount))
                    .ForMember(d => d.IssueDate, opt => opt.MapFrom(s => s.Attributes.Date))
                    .ForMember(d => d.Net, opt => opt.MapFrom(s => s.Attributes.NetAmount))
                    .ForMember(d => d.ReferenceId, opt => opt.MapFrom(s => s.Attributes.ReferenceId))
                    .ForMember(d => d.Remainder, opt => opt.MapFrom(s => s.Attributes.Remainder))
                    .ForMember(d => d.Vat, opt => opt.MapFrom(s => s.Attributes.VatAmount))
                    .ForMember(d => d.UpdatedOn, opt => opt.MapFrom(s => s.Attributes.DateUpdated));
            }
        }
    }
}
