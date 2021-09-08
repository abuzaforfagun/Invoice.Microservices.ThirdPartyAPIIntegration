using Communication.Attributes;
using Communication.Messages;

namespace InvoiceProcessor.Messages
{
    [ServiceBusQueue("process-failed-request")]
    public class ProcessOutBoxFailedCommand : DistributedCommand
    {
    }
}
