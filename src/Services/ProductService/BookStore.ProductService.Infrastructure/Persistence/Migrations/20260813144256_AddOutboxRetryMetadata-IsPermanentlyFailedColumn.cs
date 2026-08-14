using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.ProductService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxRetryMetadataIsPermanentlyFailedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPermanentlyFailed",
                table: "OutboxMessage",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPermanentlyFailed",
                table: "OutboxMessage");
        }
    }
}
