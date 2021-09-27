using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integration.Models;
using InvoiceProcessor.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FakeClient.Controllers
{
    [Route("api/Invoices")] //Swagger not working when base path changed in startup https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1253
    public class InvoicesController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly string _apiKey;

        private readonly InvoiceResponse _dummyInvoices = new(new List<InvoiceItem>
        {
            new(
                new Attributes(new DateTime(2021, 01, 01), new DateTime(2021, 01, 01), "101", "Fagun",
                    new DateTime(2023, 01, 01), "DKK", 1000.0m, 1200.0m, 0m, 200m, new DateTime(2021, 01, 15),
                    new DateTime(2021, 01, 01)), Guid.NewGuid()),
            new(
                new Attributes(new DateTime(2021, 02, 01), new DateTime(2021, 02, 01), "101", "Fagun",
                    new DateTime(2023, 01, 01), "DKK", 1000.0m, 1200.0m, 0m, 200m, new DateTime(2021, 01, 15),
                    new DateTime(2021, 02, 01)), Guid.NewGuid()),
            new(
                new Attributes(new DateTime(2021, 03, 01), new DateTime(2021, 03, 01), "101", "Fagun",
                    new DateTime(2023, 01, 01), "DKK", 1000.0m, 1200.0m, 0m, 200m, new DateTime(2021, 01, 15),
                    new DateTime(2021, 03, 01)), Guid.NewGuid()),
        });

        public InvoicesController(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;

            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException();
            }
            _apiKey = httpContextAccessor.HttpContext.Request.Headers["X-ApiKey"];
        }

        [HttpGet]
        public async Task<string> GetAll()
        {
            var invoices = await _cache.GetStringAsync(_apiKey);

            if (!string.IsNullOrWhiteSpace(invoices))
            {
                return invoices;
            }

            var jsonResult = JsonConvert.SerializeObject(_dummyInvoices);
            await _cache.SetStringAsync(_apiKey, jsonResult);
            return jsonResult;
        }

        [HttpPost]
        public async Task Add(SendInvoiceCommand invoice)
        {
            var netAmount = invoice.Lines?.Sum(l => l.UnitNetPrice);
            var vat = invoice.Lines?.Sum(l => l.VatRate);
            var grossAmount = netAmount + vat;

            _dummyInvoices.Data.Add(new InvoiceItem(
                new Attributes(invoice.Date, invoice.DueDate, invoice.MessageId.ToString(), invoice.Debtor.FirstName,
                    invoice.DueDate, invoice.Currency, (decimal)netAmount,
                    (decimal)grossAmount, 0.0m,
                    invoice.Lines.Sum(l => l.VatRate), invoice.DueDate, DateTime.Now), Guid.NewGuid()));

            var resultJson = JsonConvert.SerializeObject(_dummyInvoices);
            await _cache.SetStringAsync(_apiKey, resultJson);
        }
    }
}
