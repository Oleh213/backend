using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "deliveryOptions",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "House",
                table: "deliveryOptions",
                newName: "Address2");

            migrationBuilder.RenameColumn(
                name: "Flat",
                table: "deliveryOptions",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region",
                table: "deliveryOptions",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Address2",
                table: "deliveryOptions",
                newName: "House");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "deliveryOptions",
                newName: "Flat");
        }
    }
}
