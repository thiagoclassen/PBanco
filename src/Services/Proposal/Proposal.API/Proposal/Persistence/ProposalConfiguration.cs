using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProposalApi.Proposal.Persistence;

public class ProposalConfiguration : IEntityTypeConfiguration<Models.Proposal>
{
    public void Configure(EntityTypeBuilder<Models.Proposal> builder)
    {
        builder
            .ToTable("Proposals", "bank");

        builder
            .HasKey(x => x.Id);

        builder
            .OwnsOne(p=> p.ApprovedAmount, a =>
            {
                a.Property(p => p.Amount)
                    .HasColumnName("ApprovedAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                a.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

        builder
            .Property(p => p.ProposalStatus)
            .HasConversion<string>()
            .HasMaxLength(25);

        builder
            .Property(x => x.Requested)
            .HasDefaultValueSql("getdate()");

        builder
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("getdate()");

        builder
            .Property(p => p.UpdatedAt)
            .HasDefaultValueSql("getdate()");
    }
}
