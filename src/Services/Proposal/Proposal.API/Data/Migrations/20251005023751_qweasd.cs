using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProposalApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class qweasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedAmount",
                schema: "bank",
                table: "Proposals",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "bank",
                table: "Proposals",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "bank",
                table: "Proposals");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedAmount",
                schema: "bank",
                table: "Proposals",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
