using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoDisplay.Migrations.InfoDisplay
{
    /// <inheritdoc />
    public partial class AddName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "InfoBoards",
                type: "TEXT",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "InfoBoardItems",
                type: "TEXT",
                maxLength: 25,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "InfoBoards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "InfoBoardItems");
        }
    }
}
