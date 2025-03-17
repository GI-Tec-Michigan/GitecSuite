using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gitec.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Displays",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    HostIp = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Displays", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "DisplayThemes",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    BgColor = table.Column<string>(type: "TEXT", nullable: false),
                    TextColor = table.Column<string>(type: "TEXT", nullable: false),
                    AccentColor = table.Column<string>(type: "TEXT", nullable: false),
                    FrameColor = table.Column<string>(type: "TEXT", nullable: false),
                    FrameAccentColor = table.Column<string>(type: "TEXT", nullable: false),
                    ShowFrame = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisplayThemes", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "SchedulePackages",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveDays = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePackages", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    BoardType = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleUid = table.Column<Guid>(type: "TEXT", nullable: true),
                    DisplayThemeUid = table.Column<Guid>(type: "TEXT", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayScreenUid = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Boards_DisplayThemes_DisplayThemeUid",
                        column: x => x.DisplayThemeUid,
                        principalTable: "DisplayThemes",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Boards_Displays_DisplayScreenUid",
                        column: x => x.DisplayScreenUid,
                        principalTable: "Displays",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Boards_SchedulePackages_ScheduleUid",
                        column: x => x.ScheduleUid,
                        principalTable: "SchedulePackages",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DateRange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SchedulePackageUid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DateRange_SchedulePackages_SchedulePackageUid",
                        column: x => x.SchedulePackageUid,
                        principalTable: "SchedulePackages",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "TimeWindow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    SchedulePackageUid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeWindow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeWindow_SchedulePackages_SchedulePackageUid",
                        column: x => x.SchedulePackageUid,
                        principalTable: "SchedulePackages",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleUid = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardUid = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Elements_Boards_BoardUid",
                        column: x => x.BoardUid,
                        principalTable: "Boards",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Elements_SchedulePackages_ScheduleUid",
                        column: x => x.ScheduleUid,
                        principalTable: "SchedulePackages",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_DisplayScreenUid",
                table: "Boards",
                column: "DisplayScreenUid");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_DisplayThemeUid",
                table: "Boards",
                column: "DisplayThemeUid");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_ScheduleUid",
                table: "Boards",
                column: "ScheduleUid");

            migrationBuilder.CreateIndex(
                name: "IX_DateRange_SchedulePackageUid",
                table: "DateRange",
                column: "SchedulePackageUid");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_BoardUid",
                table: "Elements",
                column: "BoardUid");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ScheduleUid",
                table: "Elements",
                column: "ScheduleUid");

            migrationBuilder.CreateIndex(
                name: "IX_TimeWindow_SchedulePackageUid",
                table: "TimeWindow",
                column: "SchedulePackageUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DateRange");

            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "TimeWindow");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "DisplayThemes");

            migrationBuilder.DropTable(
                name: "Displays");

            migrationBuilder.DropTable(
                name: "SchedulePackages");
        }
    }
}
