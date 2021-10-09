using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Domain.Entities;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using Newtonsoft.Json;

namespace InvoiceProcessor.Infrastructure.Outbox.Services
{
    public class OutBoxService : IOutBoxService
    {
        private readonly IOutboxStorage _storage;

        public OutBoxService(IOutboxStorage storage)
        {
            _storage = storage;
        }
        
        public Task Upsert(OutBoxModel model, CancellationToken cancellationToken = new ())
        {
            var data = (model.Payload as IOutBoxValueEvent)?.Data;
            var jsonData = data == null ? "" : JsonConvert.SerializeObject(data);

            var outBoxItem = new OutboxItem(model.Guid.GetValueOrDefault(), model.CommandName, jsonData, model.Status);

            return _storage.Upsert(outBoxItem, model.Guid);
        }
    }
}
