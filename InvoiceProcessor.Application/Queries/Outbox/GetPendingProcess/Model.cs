using System;

namespace InvoiceProcessor.Application.Queries.Outbox.GetPendingProcess
{
    public partial class GetPendingProcess
    {
        public class Model
        {
            public Guid Guid { get; }
            public string CommandType { get; }
            public string Data { get; }

            public Model(Guid guid, string commandType, string data)
            {
                Guid = guid;
                CommandType = commandType;
                Data = data;
            }
        }
    }
}
