using System;

namespace InvoiceReader.Domain.Entity
{
    public record Invoice
    {
        public Guid Id { get; init; }
        public string ReferenceId { get; init; }
        public string CreditorReference { get; init; }
        public string Currency { get; init; }
        public decimal Net { get; init; }
        public decimal Gross { get; init; }
        public decimal Remainder { get; init; }
        public decimal Vat { get; init; }
        public DateTime ExpireDate { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime IssueDate { get; init; }
        public DateTime CreatedOn { get; init; }
        public DateTime UpdatedOn { get; init; }

        public Invoice()
        {
            // Required by Auto Mapper
        }
        public Invoice(Guid id, string referenceId, string creditorReference, string currency, decimal net, decimal gross, decimal remainder, decimal vat, DateTime expireDate, DateTime dueDate, DateTime issueDate, DateTime createdOn, DateTime updatedOn)
        {
            Id = id;
            ReferenceId = referenceId;
            CreditorReference = creditorReference;
            Currency = currency;
            Net = net;
            Gross = gross;
            Remainder = remainder;
            Vat = vat;
            ExpireDate = expireDate;
            DueDate = dueDate;
            IssueDate = issueDate;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }
    }
}
