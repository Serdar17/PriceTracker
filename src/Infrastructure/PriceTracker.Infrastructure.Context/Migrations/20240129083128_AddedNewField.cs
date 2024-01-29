using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceTracker.Infrastructure.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prices_sites_product_id",
                table: "prices");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "products",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "discounted_price",
                table: "prices",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "current_price",
                table: "prices",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddForeignKey(
                name: "fk_prices_products_product_id",
                table: "prices",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prices_products_product_id",
                table: "prices");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "products");

            migrationBuilder.AlterColumn<decimal>(
                name: "discounted_price",
                table: "prices",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_price",
                table: "prices",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddForeignKey(
                name: "fk_prices_sites_product_id",
                table: "prices",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
