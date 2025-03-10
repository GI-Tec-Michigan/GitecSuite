using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoDisplay.Migrations.InfoDisplay
{
    /// <inheritdoc />
    public partial class UpdateDbe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsArchived",
                table: "InfoBoards",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsArchived",
                table: "InfoBoardItems",
                newName: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "InfoBoards",
                newName: "IsArchived");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "InfoBoardItems",
                newName: "IsArchived");
        }
    }
}
