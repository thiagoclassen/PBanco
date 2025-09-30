using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProposalApi.Proposal.Model;
using ProposalApi.Proposal.Models;

namespace ProposalApi.Proposal.Persistence;

public class ProposalConfiguration : IEntityTypeConfiguration<Models.Proposal>
{
    public void Configure(EntityTypeBuilder<Models.Proposal> builder)
    {
        builder
            .ToTable("Proposals", "bank");

        builder
            .HasKey(x => x.Id);

        // builder
        //     .HasOne<ProposalStatusLookup>()
        //     .WithMany()
        //     .HasForeignKey(p => p.ProposalStatus)
        //     .HasPrincipalKey(p => p.Id);

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

// public class ProposalStatusLookupConfiguration : IEntityTypeConfiguration<ProposalStatusLookup>
// {
//     public void Configure(EntityTypeBuilder<Model.ProposalStatusLookup> builder)
//     {
//         builder
//             .ToTable("ProposalStatusLookup", "bank");
//
//         builder
//             .HasKey(x => x.Id);
//
//         // var enums = Enum.GetValues(typeof(ProposalStatus))
//         //     .Cast<ProposalStatus>()
//         //     .Select(e => new ProposalStatusLookup()
//         //     {
//         //         Id = -1 * (int)e,
//         //         Name = e.ToString()
//         //     });
//
//         builder
//             .HasData(
//                 new ProposalStatusLookup { Id = -1, Name = nameof(ProposalStatus.Pending) },
//                 new ProposalStatusLookup { Id = -2, Name = nameof(ProposalStatus.UnderReview) },
//                 new ProposalStatusLookup { Id = -3, Name = nameof(ProposalStatus.Approved) },
//                 new ProposalStatusLookup { Id = -4, Name = nameof(ProposalStatus.Rejected) },
//                 new ProposalStatusLookup { Id = -5, Name = nameof(ProposalStatus.Cancelled) }
//             );
//     }
// }