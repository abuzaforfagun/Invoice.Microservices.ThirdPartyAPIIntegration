using System.Threading.Tasks;
using InvoiceReader.Application.Queries.GetInvoices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<GetInvoices.Result> GetInvoices() => 
            _mediator.Send(new GetInvoices.Query());
    }
}
