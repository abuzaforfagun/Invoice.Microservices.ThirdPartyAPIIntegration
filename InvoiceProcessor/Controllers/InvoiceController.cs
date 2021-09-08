using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceProcessor.Application.Commands.CreateInvoiceRequest;
using MediatR;

namespace InvoiceProcessor.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] CreateInvoiceRequest.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
