using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacteristicsProduct_characteristics_CharacteristicsId",
                table: "CharacteristicsProduct");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "Memory",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "Ram",
                table: "characteristics");

            migrationBuilder.RenameColumn(
                name: "CharacteristicsId",
                table: "CharacteristicsProduct",
                newName: "CharacteristicsCharacteristicId");

            migrationBuilder.RenameColumn(
                name: "CharacteristicsId",
                table: "characteristics",
                newName: "CharacteristicId");

            migrationBuilder.AddColumn<string>(
                name: "CharacteristicName",
                table: "characteristics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Vlue",
                table: "characteristics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacteristicsProduct_characteristics_CharacteristicsCharacteristicId",
                table: "CharacteristicsProduct",
                column: "CharacteristicsCharacteristicId",
                principalTable: "characteristics",
                principalColumn: "CharacteristicId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacteristicsProduct_characteristics_CharacteristicsCharacteristicId",
                table: "CharacteristicsProduct");

            migrationBuilder.DropColumn(
                name: "CharacteristicName",
                table: "characteristics");

            migrationBuilder.DropColumn(
                name: "Vlue",
                table: "characteristics");

            migrationBuilder.RenameColumn(
                name: "CharacteristicsCharacteristicId",
                table: "CharacteristicsProduct",
                newName: "CharacteristicsId");

            migrationBuilder.RenameColumn(
                name: "CharacteristicId",
                table: "characteristics",
                newName: "CharacteristicsId");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "characteristics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "characteristics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Memory",
                table: "characteristics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ram",
                table: "characteristics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacteristicsProduct_characteristics_CharacteristicsId",
                table: "CharacteristicsProduct",
                column: "CharacteristicsId",
                principalTable: "characteristics",
                principalColumn: "CharacteristicsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
