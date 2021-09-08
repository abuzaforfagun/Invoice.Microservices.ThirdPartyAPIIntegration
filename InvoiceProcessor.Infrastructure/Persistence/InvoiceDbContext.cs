using InvoiceProcessor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessor.Infrastructure.Persistence
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options)
            : base(options) { }

        public DbSet<OutboxItem> OutBox { get; set; }
        public DbSet<OutBoxLog> OutBoxLog { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var outboxItem = builder.Entity<OutboxItem>()
                .ToTable("Outbox");
            
            outboxItem.HasKey(p => p.Guid);
            outboxItem.Property(p => p.CommandType).IsRequired();
            outboxItem.Property(p => p.Data).IsRequired();
            outboxItem.Property(p => p.Status).IsRequired();
            outboxItem.Property(p => p.CreatedOn).IsRequired();
            outboxItem.Property(p => p.ModifiedOn).IsRequired();

            builder.Entity<OutBoxLog>()
                .ToTable("OutBoxLog");

            base.OnModelCreating(builder);
        }

    }
}
