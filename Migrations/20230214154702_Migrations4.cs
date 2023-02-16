using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class Migrations4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_coments_products_ProductId",
                table: "coments");

            migrationBuilder.DropIndex(
                name: "IX_coments_ProductId",
                table: "coments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_coments_ProductId",
                table: "coments",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_coments_products_ProductId",
                table: "coments",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
