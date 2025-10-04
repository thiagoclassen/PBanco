using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditCard.API.CreditCard.Persistence;

public class CreditCardConfiguration : IEntityTypeConfiguration<Models.CreditCard>
{
    public void Configure(EntityTypeBuilder<Models.CreditCard> builder)
    {
        builder
            .ToTable("CreditCard", "bank");
        builder
            .HasKey(c => c.Id);
        builder
            .Property(p => p.CardStatus)
            .HasConversion<string>()
            .HasMaxLength(25);
        builder
            .Property(p => p.CardProvider)
            .HasConversion<string>()
            .HasMaxLength(25);
        builder
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("getdate()");

        builder
            .Property(p => p.UpdatedAt)
            .HasDefaultValueSql("getdate()");
    }
}