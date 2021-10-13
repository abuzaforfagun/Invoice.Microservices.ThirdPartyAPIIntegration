using InvoiceProcessor.Domain.Entities;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public async Task GetPendingProcess_Should_Return_EmptyList_When_Pending_Process_Unavailable()
        {
            // Arrange
            List<OutboxItem> outboxItems = new List<OutboxItem>();
            List<Model> expected = new List<Model>();

            _outboxStorageMock.Setup(x => x.GetPendingItemsAsync(new CancellationToken()))
                .ReturnsAsync(outboxItems);

            var handler = new QueryHandler(_outboxStorageMock.Object);

            // Act
            var result = await handler.Handle(new Query(), new CancellationToken());

            // Assert
            Assert.Equal(expected, result);
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

            _outboxStorageMock.Setup(x => x.GetPendingItemsAsync(new CancellationToken()))
                .ReturnsAsync(outboxItems);

            var handler = new QueryHandler(_outboxStorageMock.Object);

            // Act
            var results = await handler.Handle(new Query(), new CancellationToken());

            // Assert
            Assert.Equal(models, results, new PendingProcessEqualityComparer());
        }

        class PendingProcessEqualityComparer : IEqualityComparer<Model>
        {
            public bool Equals(Model x, Model y)
            {
                if (x.Guid == y.Guid && x.CommandType == y.CommandType && x.Data == y.Data)
                {
                    return true;
                }

                return false;
            }

            public int GetHashCode([DisallowNull] Model obj)
            {
                return obj.GetHashCode();
            }
        }
    }

}
