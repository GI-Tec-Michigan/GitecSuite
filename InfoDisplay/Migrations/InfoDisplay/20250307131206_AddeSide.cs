using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoDisplay.Migrations.InfoDisplay
{
    /// <inheritdoc />
    public partial class AddeSide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageSide",
                table: "InfoBoardItems",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSide",
                table: "InfoBoardItems");
        }
    }
}
