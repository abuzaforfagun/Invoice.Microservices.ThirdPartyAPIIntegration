using System;

namespace Communication.Messages
{
    public abstract class DistributedCommand : IDistributedCommand
    {
        private Guid _messageId;

        public Guid MessageId
        {
            get => _messageId;
            init => _messageId = value;
        }

        public void SetMessageId(Guid guid)
        {
            _messageId = guid;
        }
    }
}