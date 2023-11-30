using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceTracker.Infrastructure.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prices_site_site_id",
                table: "prices");

            migrationBuilder.DropForeignKey(
                name: "fk_sites_user_user_id",
                table: "sites");

            migrationBuilder.AddColumn<long>(
                name: "chat_id",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "fk_prices_sites_site_id",
                table: "prices",
                column: "site_id",
                principalTable: "sites",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sites_users_user_id",
                table: "sites",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prices_sites_site_id",
                table: "prices");

            migrationBuilder.DropForeignKey(
                name: "fk_sites_users_user_id",
                table: "sites");

            migrationBuilder.DropColumn(
                name: "chat_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "status",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "fk_prices_site_site_id",
                table: "prices",
                column: "site_id",
                principalTable: "sites",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_sites_user_user_id",
                table: "sites",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
