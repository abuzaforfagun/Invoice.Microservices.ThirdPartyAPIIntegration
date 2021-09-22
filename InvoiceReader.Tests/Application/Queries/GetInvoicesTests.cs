using Integration.Likvido;
using InvoiceReader.Application.Queries.GetInvoices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InvoiceReader.Tests.Application.Queries
{
    public class GetInvoicesTests
    {
        private readonly Mock<ILikvidoClient> _clientMock;

        public GetInvoicesTests()
        {
            _clientMock = new Mock<ILikvidoClient>();

            var opts = Options.Create(new MemoryDistributedCacheOptions());
        }

        [Fact]
        public async Task Get_Invoices_From_Client()
        {
            // Arrange
            var handler = new GetInvoices.QueryHandler(_clientMock.Object);

            // Act
            await handler.Handle(new GetInvoices.Query(), new CancellationToken());

            // Assert
            _clientMock.Verify(m => m.GetInvoicesAsync(), Times.Once);
        }
    }
}
