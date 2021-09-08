using System;
using System.Threading.Tasks;
using InvoiceProcessor.Application.Commands.ProcessPendingRequests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace InvoiceProcessor.Application.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class ProcessPendingInvoiceJob : IJob
    {
        private  IMediator _mediator;
        private readonly ILogger<ProcessPendingInvoiceJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ProcessPendingInvoiceJob(ILogger<ProcessPendingInvoiceJob> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"{DateTime.Now} [Processing Pending Jobs Started]" + Environment.NewLine);
            try
            {
                using var scope = _serviceProvider.CreateScope();
                _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await _mediator.Send(new ProcessPendingRequests.Command());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation($"{DateTime.Now} [Processing Pending Jobs Failed]" + Environment.NewLine);
                throw;
            }

            _logger.LogInformation($"{DateTime.Now} [Processing Pending Jobs Completed]" + Environment.NewLine);
        }
    }
}
