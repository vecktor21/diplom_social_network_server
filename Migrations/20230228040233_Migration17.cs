using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockList");

            migrationBuilder.CreateTable(
                name: "GroupBlockList",
                columns: table => new
                {
                    GroupBlockListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlockedUserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: ""),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 2, 28, 10, 2, 32, 29, DateTimeKind.Local).AddTicks(4288)),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupBlockList", x => x.GroupBlockListId);
                    table.ForeignKey(
                        name: "FK_GroupBlockList_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBlockList",
                columns: table => new
                {
                    UserBlockListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlockedUserId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: ""),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 2, 28, 10, 2, 32, 34, DateTimeKind.Local).AddTicks(361)),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlockList", x => x.UserBlockListId);
                    table.ForeignKey(
                        name: "FK_UserBlockList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2023, 2, 28, 10, 2, 32, 34, DateTimeKind.Local).AddTicks(2823));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2023, 2, 28, 10, 2, 32, 34, DateTimeKind.Local).AddTicks(2829));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2023, 2, 28, 10, 2, 32, 34, DateTimeKind.Local).AddTicks(2864));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$eBFNUCy.wCNe7cw3f0ZFHucE28AApaIByEoAgj0sG83HmFkYCch1u", new DateTime(2023, 2, 28, 10, 2, 32, 288, DateTimeKind.Local).AddTicks(5381) });

            migrationBuilder.CreateIndex(
                name: "IX_GroupBlockList_GroupId",
                table: "GroupBlockList",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlockList_UserId",
                table: "UserBlockList",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupBlockList");

            migrationBuilder.DropTable(
                name: "UserBlockList");

            migrationBuilder.CreateTable(
                name: "BlockList",
                columns: table => new
                {
                    BlockListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 1, 23, 16, 38, 12, 649, DateTimeKind.Local).AddTicks(3254)),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockList", x => x.BlockListId);
                    table.ForeignKey(
                        name: "FK_BlockList_Groups_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlockList_Users_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 23, 16, 38, 12, 663, DateTimeKind.Local).AddTicks(6517));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 23, 16, 38, 12, 663, DateTimeKind.Local).AddTicks(6526));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2023, 1, 23, 16, 38, 12, 663, DateTimeKind.Local).AddTicks(6554));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$Z2V9k2xRe3gSo/SA9d7zEeHSO78r.CwOKh8Ojkz7QNXgnn7wjbykq", new DateTime(2023, 1, 23, 16, 38, 12, 864, DateTimeKind.Local).AddTicks(4922) });

            migrationBuilder.CreateIndex(
                name: "IX_BlockList_ObjectId",
                table: "BlockList",
                column: "ObjectId");
        }
    }
}
