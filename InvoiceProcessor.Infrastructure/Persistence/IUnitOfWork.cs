using System;
using System.Threading;
using System.Threading.Tasks;
using InvoiceProcessor.Infrastructure.Repositories.BaseRepositories;

namespace InvoiceProcessor.Infrastructure.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task RunInTransactionAsync(Func<Task> taskFunc, CancellationToken cancellationToken);

        IBaseEntityRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}
