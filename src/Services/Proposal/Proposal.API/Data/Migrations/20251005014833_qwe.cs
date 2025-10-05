using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProposalApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class qwe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedOn",
                schema: "messaging",
                table: "ProcessedEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedOn",
                schema: "messaging",
                table: "ProcessedEvents");
        }
    }
}
