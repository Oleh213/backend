using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_orderLists_ProductId",
                table: "orderLists");

            migrationBuilder.CreateIndex(
                name: "IX_orderLists_ProductId",
                table: "orderLists",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_orderLists_ProductId",
                table: "orderLists");

            migrationBuilder.CreateIndex(
                name: "IX_orderLists_ProductId",
                table: "orderLists",
                column: "ProductId",
                unique: true);
        }
    }
}
