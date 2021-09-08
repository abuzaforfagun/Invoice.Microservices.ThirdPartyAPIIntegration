using System;
using Communication.Attributes;
using Communication.Messages;
using InvoiceProcessor.Domain.ValueObjects;

namespace InvoiceProcessor.Messages
{
    [ServiceBusQueue(
        name: "send-invoice", 
        deadLetterReceiver: "process-failed-request", 
        maxRetry: 1, 
        isScheduleRequired: true, 
        differedTimeInMinutes: 30)]
    public class SendInvoiceCommand : InvoiceRequest, IScheduledDistributedCommand
    {
        private Guid _messageId;

        public Guid MessageId
        {
            get => _messageId;
            init => _messageId = value;
        }

        private bool _isDiffered;

        public bool IsDiffered
        {
            get => _isDiffered;
            init => _isDiffered = value;
        }

        public void SetMessageId(Guid guid)
        {
            _messageId = guid;
        }

        public void Differ()
        {
            _isDiffered = true;
        }
    }
}
