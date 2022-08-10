using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace timesheet_calculation.Data.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeSheetManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheetManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TimeSheets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    im_UserUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSheets_Users_im_UserUserId",
                        column: x => x.im_UserUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                table: "TimeSheets",
                columns: new[] { "Id", "CheckInTime", "CheckOutTime", "UserId", "im_UserUserId" },
                values: new object[,]
                {
                    { new Guid("bd002b36-daf2-4208-b3fb-132985d41eae"), new DateTime(2022, 8, 8, 7, 58, 14, 0, DateTimeKind.Utc), new DateTime(2022, 8, 8, 17, 32, 46, 0, DateTimeKind.Utc), new Guid("d29a38b9-566c-4c95-a380-2351796764f1"), null },
                    { new Guid("e688b871-464c-4015-88f9-bd1cde3ebdfe"), new DateTime(2022, 8, 8, 7, 59, 50, 0, DateTimeKind.Utc), new DateTime(2022, 8, 8, 17, 31, 18, 0, DateTimeKind.Utc), new Guid("d29a38b9-566c-4c95-a380-2351796764f1"), null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Name" },
                values: new object[] { new Guid("d29a38b9-566c-4c95-a380-2351796764f1"), "Tuan Anh" });

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_im_UserUserId",
                table: "TimeSheets",
                column: "im_UserUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSheetManagers");

            migrationBuilder.DropTable(
                name: "TimeSheets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
