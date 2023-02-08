using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Info_orders_OrderId",
                table: "Info");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Info",
                table: "Info");

            migrationBuilder.RenameTable(
                name: "Info",
                newName: "info");

            migrationBuilder.RenameIndex(
                name: "IX_Info_OrderId",
                table: "info",
                newName: "IX_info_OrderId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_info",
                table: "info",
                column: "InfoId");

            migrationBuilder.CreateTable(
                name: "characteristics",
                columns: table => new
                {
                    CharacteristicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_characteristics", x => x.CharacteristicsId);
                    table.ForeignKey(
                        name: "FK_characteristics_products_CharacteristicsId",
                        column: x => x.CharacteristicsId,
                        principalTable: "products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_info_orders_OrderId",
                table: "info",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_info_orders_OrderId",
                table: "info");

            migrationBuilder.DropTable(
                name: "characteristics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_info",
                table: "info");

            migrationBuilder.RenameTable(
                name: "info",
                newName: "Info");

            migrationBuilder.RenameIndex(
                name: "IX_info_OrderId",
                table: "Info",
                newName: "IX_Info_OrderId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Info",
                table: "Info",
                column: "InfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Info_orders_OrderId",
                table: "Info",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
