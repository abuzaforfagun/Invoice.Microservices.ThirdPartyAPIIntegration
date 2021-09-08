using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InvoiceReader.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/error")]
        [HttpPost]
        public IActionResult Error()
        {
            var error = Problem();
            
            _logger.LogError(JsonConvert.SerializeObject(error.Value));
            return error;
        } 
    }
}
