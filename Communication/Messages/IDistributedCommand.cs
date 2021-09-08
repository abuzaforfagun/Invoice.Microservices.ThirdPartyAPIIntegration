using System;
using MediatR;

namespace Communication.Messages
{
    public interface IDistributedCommand : IRequest
    {
        Guid MessageId { get; init; }
        void SetMessageId(Guid guid);
    }
}
