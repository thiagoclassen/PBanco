using Clients.API.Client.Models;
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
            .HasQueryFilter(c => !c.IsDeleted); // exclude soft-deleted records by default

        builder
            .HasKey(x => x.Id);
        
        builder
            .HasIndex(c => c.CPF)
            .IsUnique();
        
        builder
            .HasIndex(c => c.Email)
            .IsUnique();

        builder
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("getdate()");

        builder
            .Property(p => p.UpdatedAt)
            .HasDefaultValueSql("getdate()");
    }
}