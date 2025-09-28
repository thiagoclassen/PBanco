using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clients.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class OutboxUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "messaging");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                newName: "OutboxMessages",
                newSchema: "messaging");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "messaging",
                newName: "OutboxMessages");
        }
    }
}
