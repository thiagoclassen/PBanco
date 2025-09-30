using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProposalApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class qwe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_ProposalStatusLookup_ProposalStatusLookupId",
                schema: "bank",
                table: "Proposals");

            migrationBuilder.DropTable(
                name: "ProposalStatusLookup",
                schema: "bank");

            migrationBuilder.DropIndex(
                name: "IX_Proposals_ProposalStatusLookupId",
                schema: "bank",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "ProposalStatusLookupId",
                schema: "bank",
                table: "Proposals");

            migrationBuilder.AlterColumn<string>(
                name: "ProposalStatus",
                schema: "bank",
                table: "Proposals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProposalStatus",
                schema: "bank",
                table: "Proposals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ProposalStatusLookupId",
                schema: "bank",
                table: "Proposals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProposalStatusLookup",
                schema: "bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalStatusLookup", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "bank",
                table: "ProposalStatusLookup",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -5, "Cancelled" },
                    { -4, "Rejected" },
                    { -3, "Approved" },
                    { -2, "UnderReview" },
                    { -1, "Pending" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_ProposalStatusLookupId",
                schema: "bank",
                table: "Proposals",
                column: "ProposalStatusLookupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_ProposalStatusLookup_ProposalStatusLookupId",
                schema: "bank",
                table: "Proposals",
                column: "ProposalStatusLookupId",
                principalSchema: "bank",
                principalTable: "ProposalStatusLookup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
