using System.Threading.Tasks;
using InvoiceProcessor.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceProcessor.Controllers
{
    [Route("internal-api/[controller]")]
    [ApiController]
    public class OutboxController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OutboxController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<GetProcessStatusResult> GetProcessCompletionStatus([FromQuery] GetProcessStatusQuery query)
        {
            return _mediator.Send(query);
        }
    }
}
