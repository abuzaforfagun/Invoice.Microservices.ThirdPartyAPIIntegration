using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessor.Infrastructure.Repositories.BaseRepositories
{
    public interface IEntityRepository<TDbContext, TEntity> : IBaseEntityRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
    }
}
