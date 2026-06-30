using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eskon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIsSharedAndBedCountToHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BedCount",
                table: "House",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "House",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 1,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 2,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 3,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 4,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 5,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 6,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 7,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 8,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 9,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });

            migrationBuilder.UpdateData(
                table: "House",
                keyColumn: "HouseId",
                keyValue: 10,
                columns: new[] { "BedCount", "IsShared" },
                values: new object[] { 0, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BedCount",
                table: "House");

            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "House");
        }
    }
}
