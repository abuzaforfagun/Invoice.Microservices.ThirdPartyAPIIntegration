using System;
using InvoiceProcessor.Domain.Enums;

namespace InvoiceProcessor.Domain.Entities
{
    public class OutboxItem
    {
        private OutBoxStatus _status;
        private DateTime _modifiedOn;
        public Guid? Guid { get; }
        public string CommandType { get; }
        public string Data { get; }

        public OutBoxStatus Status
        {
            get => _status;
            init => _status = value;
        }
        public DateTime CreatedOn { get; }

        public DateTime ModifiedOn
        {
            get => _modifiedOn;
            init => _modifiedOn = value;
        }

        public OutboxItem()
        {
            // Required by Auto Mapper
        }

        public OutboxItem(Guid? guid, string commandType, string data, OutBoxStatus status)
        {
            CommandType = commandType;
            Data = data;
            CreatedOn = DateTime.UtcNow;
            Status = status;
            ModifiedOn = DateTime.UtcNow;
        }
        
        public void UpdateStatus(OutBoxStatus status)
        {
            _status = status;
            _modifiedOn = DateTime.UtcNow;
        }
    }
}
