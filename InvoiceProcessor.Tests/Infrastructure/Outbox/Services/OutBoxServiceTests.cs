using InvoiceProcessor.Domain.Entities;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.Events;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using InvoiceProcessor.Infrastructure.Outbox;
using InvoiceProcessor.Infrastructure.Outbox.Services;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InvoiceProcessor.Tests.Infrastructure.Outbox.Services
{
    public class OutBoxServiceTests
    {
        private readonly Mock<IOutboxStorage> _storageMock;
        private readonly OutBoxService _outBoxServiceMock;

        public OutBoxServiceTests()
        {
            _storageMock = new Mock<IOutboxStorage>();
            _outBoxServiceMock = new OutBoxService(_storageMock.Object); 
        }

        [Fact]
        public async Task Should_Upsert()
        {
            // Assign 
            var guid = Guid.NewGuid();
            var payload = new OutBoxEvent(guid);
            var model = new OutBoxModel(payload, OutBoxStatus.Pending, "abcd");

            // Act
            await _outBoxServiceMock.Upsert(model);

            // Assert
            _storageMock.Verify(
                x => x.Upsert(It.IsAny<OutboxItem>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
