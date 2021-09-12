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

            public Result(List<Invoice> invoices)
            {
                Data = invoices;
            }
        }
    }
}
