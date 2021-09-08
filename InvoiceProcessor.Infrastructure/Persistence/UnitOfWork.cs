using System;
using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Infrastructure.Repositories;
using InvoiceProcessor.Infrastructure.Repositories.BaseRepositories;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessor.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InvoiceDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(InvoiceDbContext dbContext, IServiceProvider serviceProvider, ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        
        public async Task RunInTransactionAsync(Func<Task> taskFunc, CancellationToken cancellationToken)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await taskFunc();
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public IBaseEntityRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var repositoryType = typeof(IEntityRepository<InvoiceDbContext, TEntity>);
            return (IBaseEntityRepository<TEntity>)_serviceProvider.GetService(repositoryType);
        }
    }
}
