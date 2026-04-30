using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eskon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHouseTypeAndIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "House",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 1,
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 2,
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 3,
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 4,
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 5,
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 6,
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 7,
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 8,
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 9,
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 10,
                column: "Type",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_House_Type",
                table: "House",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_House_Type",
                table: "House");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "House");
        }
    }
}
