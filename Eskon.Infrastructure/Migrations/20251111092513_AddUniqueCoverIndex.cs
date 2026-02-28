#nullable disable

namespace Eskon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueCoverIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MediaItem_HouseId_IsCover",
                table: "MediaItem",
                columns: new[] { "HouseId", "IsCover" },
                unique: true,
                filter: "[IsCover] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaItem_HouseId_IsCover",
                table: "MediaItem");
        }
    }
}
