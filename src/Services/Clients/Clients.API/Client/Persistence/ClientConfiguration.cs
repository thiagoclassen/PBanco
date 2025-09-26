using Clients.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clients.API.Client.Persistence;

public class ClientConfiguration : IEntityTypeConfiguration<BankClient>
{
    public void Configure(EntityTypeBuilder<BankClient> builder)
    {
        builder
            .ToTable("Clients", "bank");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("getdate()");
        
        builder
            .Property(p => p.UpdatedAt)
            .HasDefaultValueSql("getdate()");
    }
}