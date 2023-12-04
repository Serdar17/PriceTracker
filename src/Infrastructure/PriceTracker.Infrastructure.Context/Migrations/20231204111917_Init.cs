using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PriceTracker.Infrastructure.Context.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    last_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    username = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    market_place_name = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    link = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    current_price = table.Column<decimal>(type: "numeric", nullable: false),
                    discounted_price = table.Column<decimal>(type: "numeric", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prices", x => x.id);
                    table.ForeignKey(
                        name: "fk_prices_sites_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prices_product_id",
                table: "prices",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_user_id",
                table: "products",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_username",
                table: "users",
                column: "username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prices");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
