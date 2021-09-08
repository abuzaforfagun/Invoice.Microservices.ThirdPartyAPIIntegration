using System;
using Communication.Attributes;
using Communication.Messages;
using InvoiceProcessor.Domain.Enums;
using MediatR;

namespace InvoiceProcessor.Messages
{
    [HttpEndPoint("http://localhost:5001/internal-api/outbox")]
    public record GetProcessStatusQuery : IDistributedQuery, IRequest<GetProcessStatusResult>
    {
        public Guid MessageId { get; init; }

        public GetProcessStatusQuery(Guid messageId)
        {
            MessageId = messageId;
        }

        public GetProcessStatusQuery()
        {
            // Required by API to bind params
        }
    }

    public class GetProcessStatusResult
    {
        public OutBoxStatus Status { get; init; }

        public GetProcessStatusResult(OutBoxStatus status)
        {
            Status = status;
        }
    }
}
