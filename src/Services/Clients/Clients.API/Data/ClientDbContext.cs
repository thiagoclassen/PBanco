using BuildingBlocks.Outbox.Models;
using Clients.API.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace Clients.API.Data;

public class ClientDbContext : DbContext
{
    public DbSet<BankClient> Clients { get; init; }
    public DbSet<OutboxMessage> OutboxMessages { get; init; }

    public ClientDbContext()
    {
    }

    public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options)
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientDbContext).Assembly);

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