using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoDisplay.Migrations.InfoDisplay
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoBoards",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoBoards", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "InfoBoardItems",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    JsonContent = table.Column<string>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    InfoBoardUid = table.Column<Guid>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ImageAlt = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ImageCaption = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    MarkdownContent = table.Column<string>(type: "TEXT", nullable: true),
                    RssFeedUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    TextContent = table.Column<string>(type: "TEXT", nullable: true),
                    VideoSource = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    VideoAlt = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    VideoCaption = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoBoardItems", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_InfoBoardItems_InfoBoards_InfoBoardUid",
                        column: x => x.InfoBoardUid,
                        principalTable: "InfoBoards",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfoBoardItems_InfoBoardUid",
                table: "InfoBoardItems",
                column: "InfoBoardUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoBoardItems");

            migrationBuilder.DropTable(
                name: "InfoBoards");
        }
    }
}
