using System.Collections.Generic;
using Integration.Models;
using InvoiceReader.Domain.Entity;

namespace InvoiceReader.Application.Queries.GetInvoices
{
    public partial class GetInvoices
    {
        public record Result
        {
            public List<Invoice> Data { get; set; }

            public Result(List<InvoiceItem> invoiceItems)
            {
                Data = new List<Invoice>();

                foreach(var invoiceItem in invoiceItems)
                {
                    var invoice = new Invoice
                    (
                        id : invoiceItem.Id,
                        referenceId : invoiceItem.Attributes.ReferenceId,
                        creditorReference : invoiceItem.Attributes.CreditorReference,
                        currency : invoiceItem.Attributes.Currency,
                        net : invoiceItem.Attributes.NetAmount,
                        gross : invoiceItem.Attributes.GrossAmount,
                        remainder : invoiceItem.Attributes.Remainder,
                        vat : invoiceItem.Attributes.VatAmount,
                        expireDate : invoiceItem.Attributes.InvoiceExpirationDate,
                        dueDate : invoiceItem.Attributes.DueDate,
                        issueDate : invoiceItem.Attributes.Date,
                        createdOn : invoiceItem.Attributes.DateCreated,
                        updatedOn : invoiceItem.Attributes.DateUpdated
                    );

                    Data.Add(invoice);
                }
            }
        }
    }
}
