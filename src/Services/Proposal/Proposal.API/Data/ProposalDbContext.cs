using BuildingBlocks.Outbox.Models;
using BuildingBlocks.ProcessedEvents.Models;
using Microsoft.EntityFrameworkCore;

namespace ProposalApi.Data;

public class ProposalDbContext : DbContext
{
    public DbSet<Proposal.Models.Proposal> Proposals { get; init; }
    public DbSet<OutboxMessage> OutboxMessages { get; init; }
    public DbSet<ProcessedEvent> ProcessedEvents { get; init; }
    
    public ProposalDbContext()
    {
    }

    public ProposalDbContext(DbContextOptions<ProposalDbContext> options) : base(options)
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProposalDbContext).Assembly);
        
        modelBuilder.Entity<OutboxMessage>().ToTable("OutboxMessages", "messaging");
        
        modelBuilder.Entity<ProcessedEvent>().HasKey(x => x.EventId);
        modelBuilder.Entity<ProcessedEvent>().ToTable("ProcessedEvents", "messaging");

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
                        entity.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }
    }
}