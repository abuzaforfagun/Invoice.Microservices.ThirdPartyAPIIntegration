using System;
using System.Threading;
using System.Threading.Tasks;
using Communication.Sender;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Messages;
using InvoiceReader.Application.Commands.InvalidateCache;
using InvoiceReader.Application.Events;
using InvoiceReader.Messages;
using MediatR;
using Moq;
using Xunit;

namespace InvoiceReader.Tests.Application.Commands
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
        public async Task Invalidate_Cache_When_Request_Processed()
        {
            // Arrange
            _distributedSender
                .Setup(m => m.GetAsync<GetProcessStatusResult>(It.IsAny<GetProcessStatusQuery>()))
                .ReturnsAsync(new GetProcessStatusResult(OutBoxStatus.Processed));
            var handler = new NewInvoiceAddedHandler(_mediator.Object, _distributedSender.Object);

            // Act
            await handler.Handle(new NewInvoiceAdded(Guid.NewGuid()));

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<InvalidateCache.Command>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Do_Nothing_When_Request_Failed()
        {
            // Arrange
            _distributedSender
                .Setup(m => m.GetAsync<GetProcessStatusResult>(It.IsAny<GetProcessStatusQuery>()))
                .ReturnsAsync(new GetProcessStatusResult(OutBoxStatus.Failed));
            var handler = new NewInvoiceAddedHandler(_mediator.Object, _distributedSender.Object);

            // Act
            await handler.Handle(new NewInvoiceAdded(Guid.NewGuid()));

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<InvalidateCache.Command>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Throw_Exception_When_Request_Is_Processing()
        {
            // Arrange
            _distributedSender
                .Setup(m => m.GetAsync<GetProcessStatusResult>(It.IsAny<GetProcessStatusQuery>()))
                .ReturnsAsync(new GetProcessStatusResult(OutBoxStatus.Processing));
            var handler = new NewInvoiceAddedHandler(_mediator.Object, _distributedSender.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(new NewInvoiceAdded(Guid.NewGuid())));
        }
    }
}
