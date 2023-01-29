using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatee18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deliveryOptions_users_DeliveryOptionsId",
                table: "deliveryOptions");

            migrationBuilder.CreateIndex(
                name: "IX_deliveryOptions_UserId",
                table: "deliveryOptions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_deliveryOptions_users_UserId",
                table: "deliveryOptions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deliveryOptions_users_UserId",
                table: "deliveryOptions");

            migrationBuilder.DropIndex(
                name: "IX_deliveryOptions_UserId",
                table: "deliveryOptions");

            migrationBuilder.AddForeignKey(
                name: "FK_deliveryOptions_users_DeliveryOptionsId",
                table: "deliveryOptions",
                column: "DeliveryOptionsId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
