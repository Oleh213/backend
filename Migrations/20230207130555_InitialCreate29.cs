using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "characteristicsProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "characteristicsProducts",
                columns: table => new
                {
                    CharacteristicsProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacteristicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_characteristicsProducts", x => x.CharacteristicsProductId);
                    table.ForeignKey(
                        name: "FK_characteristicsProducts_characteristics_CharacteristicsId",
                        column: x => x.CharacteristicsId,
                        principalTable: "characteristics",
                        principalColumn: "CharacteristicsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_characteristicsProducts_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_characteristicsProducts_CharacteristicsId",
                table: "characteristicsProducts",
                column: "CharacteristicsId");

            migrationBuilder.CreateIndex(
                name: "IX_characteristicsProducts_ProductId",
                table: "characteristicsProducts",
                column: "ProductId");
        }
    }
}
