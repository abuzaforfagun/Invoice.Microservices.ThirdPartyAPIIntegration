using Integration.Likvido;
using Integration.Models;
using InvoiceReader.Application;
using InvoiceReader.Application.Queries.GetInvoices;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InvoiceReader.Tests.Application.Queries
{
    public class GetInvoicesTests
    {
        private readonly Mock<ILikvidoClient> _clientMock;
        private readonly MemoryDistributedCache _cache;

        public GetInvoicesTests()
        {
            _clientMock = new Mock<ILikvidoClient>();

            var opts = Options.Create(new MemoryDistributedCacheOptions());
            _cache = new MemoryDistributedCache(opts);
        }

        [Fact]
        public async Task Get_Invoices_From_Cache_When_Available()
        {
            // Arrange
            var invoiceItem = new List<InvoiceItem>();
            var emptyResult = new GetInvoices.Result(invoiceItem);
            var jsonResult = JsonConvert.SerializeObject(emptyResult);
            await _cache.SetStringAsync(Constants.GetInvoicesKey, jsonResult);
            var handler = new GetInvoices.QueryHandler(_clientMock.Object, _cache);

            // Act
            await handler.Handle(new GetInvoices.Query(), new CancellationToken());

            // Assert
            _clientMock.Verify(m => m.GetInvoicesAsync(), Times.Never);
        }

        [Fact]
        public async Task Get_Invoices_From_Client_When_No_Cache_Available()
        {
            // Arrange
            var handler = new GetInvoices.QueryHandler(_clientMock.Object, _cache);

            // Act
            await handler.Handle(new GetInvoices.Query(), new CancellationToken());

            // Assert
            _clientMock.Verify(m => m.GetInvoicesAsync(), Times.Once);
        }
    }
}
