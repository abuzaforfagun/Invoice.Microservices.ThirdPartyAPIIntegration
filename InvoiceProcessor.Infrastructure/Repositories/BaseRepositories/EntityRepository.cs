using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceProcessor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessor.Infrastructure.Repositories.BaseRepositories
{
    public class EntityRepository<TDbContext, TEntity> : IEntityRepository<TDbContext, TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        private readonly DbSet<TEntity> _dbSet;

        public EntityRepository(InvoiceDbContext dbContext)
        {
            _dbSet = dbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entity)
        {
            return _dbSet.AddRangeAsync(entity);
        }

        public TEntity Get<TIdentity>(TIdentity id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> GetAsync<TIdentity>(TIdentity id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Attach(TEntity entity)
        {
            _dbSet.Attach(entity);
        }

        public void AttachRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AttachRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet.AsQueryable();
        }
    }
}