using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Integration.Likvido;
using InvoiceReader.Application;
using InvoiceReader.Application.Queries.GetInvoices;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace InvoiceReader.Tests.Application.Queries
{
    public class GetInvoicesTests
    {
        private readonly Mock<ILikvidoClient> _clientMock;
        private readonly Mapper _mapper;
        private readonly MemoryDistributedCache _cache;

        public GetInvoicesTests()
        {
            _clientMock = new Mock<ILikvidoClient>();

            var mapperCfg = new MapperConfiguration(cfg => cfg.AddProfile(new GetInvoices.MapperConfiguration()));
            _mapper = new Mapper(mapperCfg);

            var opts = Options.Create(new MemoryDistributedCacheOptions());
            _cache = new MemoryDistributedCache(opts);
        }

        [Fact]
        public async Task Get_Invoices_From_Cache_When_Available()
        {
            // Arrange
            var emptyResult = new GetInvoices.Result();
            var jsonResult = JsonConvert.SerializeObject(emptyResult);
            await _cache.SetStringAsync(Constants.GetInvoicesKey, jsonResult);
            var handler = new GetInvoices.QueryHandler(_clientMock.Object, _mapper, _cache);

            // Act
            await handler.Handle(new GetInvoices.Query(), new CancellationToken());

            // Assert
            _clientMock.Verify(m => m.GetInvoicesAsync(), Times.Never);
        }

        [Fact]
        public async Task Get_Invoices_From_Client_When_No_Cache_Available()
        {
            // Arrange
            var handler = new GetInvoices.QueryHandler(_clientMock.Object, _mapper, _cache);

            // Act
            await handler.Handle(new GetInvoices.Query(), new CancellationToken());

            // Assert
            _clientMock.Verify(m => m.GetInvoicesAsync(), Times.Once);
        }
    }
}
