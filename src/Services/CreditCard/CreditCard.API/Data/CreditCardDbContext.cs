using BuildingBlocks.Outbox.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditCard.API.Data;

public class CreditCardDbContext : DbContext
{
    public DbSet<CreditCard.Models.CreditCard> CreditCards { get; init; }
    public DbSet<OutboxMessage> OutboxMessages { get; init; }

    public CreditCardDbContext()
    {
    }

    public CreditCardDbContext(DbContextOptions<CreditCardDbContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .LogTo(Console.WriteLine,
                [DbLoggerCategory.Database.Command.Name],
                LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CreditCardDbContext).Assembly);
        
        modelBuilder.Entity<OutboxMessage>()
            .ToTable("OutboxMessages", "messaging");

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        UpdateAtTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAtTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAtTimestamps()
    {
        var modifiedEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified && e.Metadata.GetProperties().Any(p => p.Name == "UpdatedAt"));

        foreach (var entity in modifiedEntities)
        {
            entity.Property("UpdatedAt").CurrentValue = DateTime.Now;
        }
    }
}