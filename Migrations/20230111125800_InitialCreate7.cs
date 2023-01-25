using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "cartItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_ProductId1",
                table: "cartItems",
                column: "ProductId1",
                unique: true,
                filter: "[ProductId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_cartItems_products_ProductId1",
                table: "cartItems",
                column: "ProductId1",
                principalTable: "products",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cartItems_products_ProductId1",
                table: "cartItems");

            migrationBuilder.DropIndex(
                name: "IX_cartItems_ProductId1",
                table: "cartItems");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "cartItems");
        }
    }
}
