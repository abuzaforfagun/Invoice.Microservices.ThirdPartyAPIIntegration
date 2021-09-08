using System;
using InvoiceProcessor.Domain.Enums;

namespace InvoiceProcessor.Domain.Entities
{
    public class OutBoxLog
    {
        public int Id { get; set; }
        public Guid OutBoxId { get; set; }
        public OutboxItem OutBox{ get; set; }
        public OutBoxStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}