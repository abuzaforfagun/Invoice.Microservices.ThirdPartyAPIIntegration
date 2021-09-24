using InvoiceProcessor.Domain.Entities;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static InvoiceProcessor.Application.Queries.Outbox.GetPendingProcess.GetPendingProcess;

namespace InvoiceProcessor.Tests.Application.Queries.Outbox
{
    public class GetPendingProcessTests
    {
        private readonly Mock<IOutboxStorage> _outboxStorageMock;

        public GetPendingProcessTests()
        {
            _outboxStorageMock = new Mock<IOutboxStorage>();
        }

        [Fact]
        public async Task GetPendingProcess_Should_Return_Empty_When_Pending_Process_Unavailable()
        {
            // Arrange
            List<OutboxItem> outboxItems = null;
            List<Model> models = null;
            var handler = new QueryHandler(_outboxStorageMock.Object);
            _outboxStorageMock.Setup(x => x.GetPendingItemsAsync(new CancellationToken()))
                .ReturnsAsync(outboxItems);

            // Act
            var result = await handler.Handle(new Query(), new CancellationToken());

            // Assert
            Assert.Equal(models, result);
        }


        [Fact]
        public async Task GetPendingProcess_Should_Return_List_When_Pending_Process_Available()
        {
            // Arrange
            var guid = Guid.Empty;

            var outboxItems = new List<OutboxItem>
            {
                new OutboxItem(guid: guid, commandType: "1", data: "a", status: OutBoxStatus.Pending),
                new OutboxItem(guid: guid, commandType: "2", data: "b", status: OutBoxStatus.Pending),
                new OutboxItem(guid: guid, commandType: "3", data: "c", status: OutBoxStatus.Pending)
            };

            var models = new List<Model>
            {
                new Model(guid: guid, commandType: "1", data: "a"),
                new Model(guid: guid, commandType: "2", data: "b"),
                new Model(guid: guid, commandType: "3", data: "c")
            };

            var handler = new QueryHandler(_outboxStorageMock.Object);
            _outboxStorageMock.Setup(x => x.GetPendingItemsAsync(new CancellationToken()))
                .ReturnsAsync(outboxItems);

            // Act
            var results = await handler.Handle(new Query(), new CancellationToken());

            // Assert
            Assert.NotStrictEqual(models, results);
        }

    }
}
