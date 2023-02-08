using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characteristics_products_CharacteristicsId",
                table: "characteristics");

            migrationBuilder.CreateIndex(
                name: "IX_characteristics_ProductId",
                table: "characteristics",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_characteristics_products_ProductId",
                table: "characteristics",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characteristics_products_ProductId",
                table: "characteristics");

            migrationBuilder.DropIndex(
                name: "IX_characteristics_ProductId",
                table: "characteristics");

            migrationBuilder.AddForeignKey(
                name: "FK_characteristics_products_CharacteristicsId",
                table: "characteristics",
                column: "CharacteristicsId",
                principalTable: "products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
