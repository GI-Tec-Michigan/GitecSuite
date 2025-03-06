using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoDisplay.Migrations.InfoDisplay
{
    /// <inheritdoc />
    public partial class AddedVideoStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoStory",
                table: "InfoBoardItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoStory",
                table: "InfoBoardItems");
        }
    }
}
