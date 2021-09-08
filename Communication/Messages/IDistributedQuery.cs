using System;

namespace Communication.Messages
{
    public interface IDistributedQuery
    {
        Guid MessageId { get; init; }
    }
}