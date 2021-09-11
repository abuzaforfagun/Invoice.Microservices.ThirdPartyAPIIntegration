using System;
using System.Threading;
using System.Threading.Tasks;
using Communication.Sender;
using InvoiceReader.Application.Commands.InvalidateCache;
using InvoiceReader.Application.Events;
using InvoiceReader.Messages;
using MediatR;
using Moq;
using Xunit;

namespace InvoiceReader.Tests.Application.Events
{
    public class NewInvoiceAddedHandlerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IDistributedSender> _distributedSender;

        public NewInvoiceAddedHandlerTests()
        {
            _mediator = new Mock<IMediator>();
            _distributedSender = new Mock<IDistributedSender>();
        }

        [Fact]
        public async Task Should_Invalidate_Cache()
        {
            // Arrange
            var handler = new NewInvoiceAddedHandler(_mediator.Object);

            // Act
            await handler.Handle(new NewInvoiceAdded(Guid.NewGuid()));

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<InvalidateCache.Command>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
