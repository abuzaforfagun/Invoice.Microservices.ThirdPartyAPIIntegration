using System;
using System.Collections.Generic;

namespace InvoiceProcessor.Domain.ValueObjects
{
    public class InvoiceRequest : IValueObject
    {
        public DateTime Date { get; init; }
        public DateTime DueDate { get; init; }
        public Debtor Debtor { get; init; }
        public string Currency { get; init; }
        public int CampaignInitialRequest { get; init; }
        public List<Line> Lines { get; init; }
    }
}
