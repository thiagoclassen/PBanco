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
            .OwnsOne(p => p.ExpensesLimit, a =>
            {
                a.Property(p => p.Amount)
                    .HasColumnName("ExpensesLimit")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                a.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });
        
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