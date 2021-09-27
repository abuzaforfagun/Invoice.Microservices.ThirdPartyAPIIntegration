using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Gateway.Middleware
{
    public class CorrelationMiddleware
    {
        private const string CorrelationIdHeaderKey = "x-correlation-id";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationMiddleware> _logger;
        public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            string correlationId = null;
            if (httpContext.Request.Headers.TryGetValue(
                CorrelationIdHeaderKey, out StringValues correlationIds))
            {
                correlationId = correlationIds.FirstOrDefault(k =>
                    k.Equals(CorrelationIdHeaderKey));
                _logger.LogInformation($"CorrelationId from Request Header: { correlationId}");
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                httpContext.Request.Headers.Add(CorrelationIdHeaderKey,
                    correlationId);
                _logger.LogInformation($"Generated CorrelationId: { correlationId}");
            }
            await _next.Invoke(httpContext);
        }
    }
}
