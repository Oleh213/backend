using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_orderLists_ProductId",
                table: "products");

            migrationBuilder.CreateIndex(
                name: "IX_orderLists_ProductId",
                table: "orderLists",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_orderLists_products_ProductId",
                table: "orderLists",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderLists_products_ProductId",
                table: "orderLists");

            migrationBuilder.DropIndex(
                name: "IX_orderLists_ProductId",
                table: "orderLists");

            migrationBuilder.AddForeignKey(
                name: "FK_products_orderLists_ProductId",
                table: "products",
                column: "ProductId",
                principalTable: "orderLists",
                principalColumn: "OrderListId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
