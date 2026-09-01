using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.ProductService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTraceContextToOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TraceParent",
                table: "OutboxMessage",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraceState",
                table: "OutboxMessage",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TraceParent",
                table: "OutboxMessage");

            migrationBuilder.DropColumn(
                name: "TraceState",
                table: "OutboxMessage");
        }
    }
}
