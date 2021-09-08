using System;
using System.Collections.Generic;

namespace Integration.Models
{
    public record InvoiceResponse(List<InvoiceItem> Data);

    public record InvoiceItem(Attributes Attributes, Guid Id);

    public record Attributes(
        DateTime DateCreated, 
        DateTime DateUpdated, 
        string ReferenceId, 
        string CreditorReference,
        DateTime InvoiceExpirationDate, 
        string Currency, 
        decimal NetAmount, 
        decimal GrossAmount, 
        decimal Remainder,
        decimal VatAmount, 
        DateTime DueDate, 
        DateTime Date);
}
