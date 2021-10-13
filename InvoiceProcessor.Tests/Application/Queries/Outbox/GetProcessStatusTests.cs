using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.GetProcessStatus;
using InvoiceProcessor.Messages;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static InvoiceProcessor.Application.Queries.Outbox.GetProcessStatus.GetProcessStatus;

namespace InvoiceProcessor.Tests.Application.Queries.Outbox
{
    public class GetProcessStatusTests
    {
        private readonly Mock<IProcessStatusRepository> _processStatusRepositoryMock;

        public GetProcessStatusTests()
        {
            _processStatusRepositoryMock = new Mock<IProcessStatusRepository>();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task GetProcessStatus_Should_Return_Process_Status(OutBoxStatus status)
        {
            // Arrange
            _processStatusRepositoryMock.Setup(x => x.GetProcessStatus(It.IsAny<Guid>()))
                .ReturnsAsync(status);

            var handler = new QueryHandler(_processStatusRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetProcessStatusQuery(), new CancellationToken());

            // Arrange
            Assert.Equal(status, result.Status);
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[]{ OutBoxStatus.Pending },
                new object[]{ OutBoxStatus.Processing },
                new object[]{ OutBoxStatus.Processed },
                new object[]{ OutBoxStatus.Failed }
            };
    }
}
