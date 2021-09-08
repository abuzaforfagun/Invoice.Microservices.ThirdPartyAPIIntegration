using System;
using System.Collections.Generic;
using InvoiceProcessor.Application.Commands.Outbox.Events;
using InvoiceProcessor.Application.Factories;
using InvoiceProcessor.Domain.Enums;
using InvoiceProcessor.Domain.ValueObjects;
using Xunit;

namespace InvoiceProcessor.Tests.Application.Factories
{
    public class OutBoxFactoryTest
    {
        [Fact]
        public void GetOutBoxCommand_Should_Work_For_CreateInvoiceRequestCommand()
        {
            var factory = new OutBoxFactory();
            var data = new InvoiceRequest
            {
                CampaignInitialRequest = 6,
                Currency = "DKK",
                Date = DateTime.UtcNow,
                Debtor = new Debtor("Jhon", "Sina", "jhon@em.com", "01677813190", 6, "Dhaka, Bangladesh", "1212",
                    "Dhaka"),
                DueDate = DateTime.UtcNow,
                Lines = new List<Line>()
            };

            var result = factory.GetOutBoxEvent(OutBoxStatus.Pending, data);

            Assert.Equal(OutBoxStatus.Pending, result.Status);
            Assert.Equal(data, ((InvoiceRequestCreated) result.Data).Data);
        }

        [Fact]
        public void GetOutBoxCommand_Should_Work_For_ProcessInvoiceRequestCommand()
        {
            var factory = new OutBoxFactory();
            var guid = Guid.NewGuid();

            var result = factory.GetOutBoxEvent(OutBoxStatus.Processing, guid: guid);

            Assert.Equal(OutBoxStatus.Processing, result.Status);
            Assert.Equal(guid, result.Data.MessageId);
        }

        [Fact]
        public void GetOutBoxCommand_Should_Work_For_CompleteInvoiceRequestCommand()
        {
            var factory = new OutBoxFactory();
            var guid = Guid.NewGuid();

            var result = factory.GetOutBoxEvent(OutBoxStatus.Processed, guid: guid);

            Assert.Equal(OutBoxStatus.Processed, result.Status);
            Assert.Equal(guid, result.Data.MessageId);
        }
    }
}
