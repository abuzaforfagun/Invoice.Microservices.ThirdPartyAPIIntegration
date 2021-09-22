using Integration.Likvido;
using InvoiceReader.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceReader.Application.Queries.GetInvoices
{
    public partial class GetInvoices
    {
        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly ILikvidoClient _likvidoClient;

            public QueryHandler(ILikvidoClient likvidoClient)
            {
                _likvidoClient = likvidoClient;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var clientResponse = await _likvidoClient.GetInvoicesAsync();

                var invoices = clientResponse?.Data?.Select(invoiceItem => new Invoice(
                    id: invoiceItem.Id,
                    referenceId: invoiceItem.Attributes.ReferenceId,
                    creditorReference: invoiceItem.Attributes.CreditorReference,
                    currency: invoiceItem.Attributes.Currency,
                    net: invoiceItem.Attributes.NetAmount,
                    gross: invoiceItem.Attributes.GrossAmount,
                    remainder: invoiceItem.Attributes.Remainder,
                    vat: invoiceItem.Attributes.VatAmount,
                    expireDate: invoiceItem.Attributes.InvoiceExpirationDate,
                    dueDate: invoiceItem.Attributes.DueDate,
                    issueDate: invoiceItem.Attributes.Date,
                    createdOn: invoiceItem.Attributes.DateCreated,
                    updatedOn: invoiceItem.Attributes.DateUpdated))
                    .ToList();

                return new Result(invoices);
            }
        }
    }
}
