using System;
using Communication.Attributes;
using Communication.Messages;

namespace InvoiceReader.Messages
{
    [ServiceBusQueue("new-invoice-added")]
    public class NewInvoiceAdded : DistributedCommand
    {
        public NewInvoiceAdded(Guid messageId)
        {
            MessageId = messageId;
        }
    }
}
