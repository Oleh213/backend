using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characteristics_products_ProductId",
                table: "characteristics");

            migrationBuilder.DropIndex(
                name: "IX_characteristics_ProductId",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "characteristics");

            migrationBuilder.AddColumn<string>(
                name: "Ram",
                table: "characteristics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CharacteristicsProduct",
                columns: table => new
                {
                    CharacteristicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicsProduct", x => new { x.CharacteristicsId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CharacteristicsProduct_characteristics_CharacteristicsId",
                        column: x => x.CharacteristicsId,
                        principalTable: "characteristics",
                        principalColumn: "CharacteristicsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacteristicsProduct_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacteristicsProduct_ProductId",
                table: "CharacteristicsProduct",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacteristicsProduct");

            migrationBuilder.DropColumn(
                name: "Ram",
                table: "characteristics");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "characteristics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
