using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 15, 26, 37, 534, DateTimeKind.Local).AddTicks(1212),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 26, 15, 26, 4, 457, DateTimeKind.Local).AddTicks(9322));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 15, 26, 37, 545, DateTimeKind.Local).AddTicks(7569));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 15, 26, 37, 545, DateTimeKind.Local).AddTicks(7579));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2022, 11, 26, 15, 26, 37, 545, DateTimeKind.Local).AddTicks(7626));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$lQoD9O130DtKzTDhcf0JRehM73u/xm40iqpYe1liG6hgJhspHLv.G", new DateTime(2022, 11, 26, 15, 26, 37, 856, DateTimeKind.Local).AddTicks(4827) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 15, 26, 4, 457, DateTimeKind.Local).AddTicks(9322),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 26, 15, 26, 37, 534, DateTimeKind.Local).AddTicks(1212));

            migrationBuilder.CreateTable(
                name: "CommentAttachments",
                columns: table => new
                {
                    CommentAttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    FileID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentAttachments", x => x.CommentAttachmentId);
                    table.ForeignKey(
                        name: "FK_CommentAttachments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentAttachments_Files_FileID",
                        column: x => x.FileID,
                        principalTable: "Files",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 15, 26, 4, 499, DateTimeKind.Local).AddTicks(8204));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 15, 26, 4, 499, DateTimeKind.Local).AddTicks(8283));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2022, 11, 26, 15, 26, 4, 499, DateTimeKind.Local).AddTicks(8463));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$GPm8yXvFsIgYGYF2ZQai1e5DOJxK1IcKYXekjZMuivBb2wXjUZdhe", new DateTime(2022, 11, 26, 15, 26, 4, 915, DateTimeKind.Local).AddTicks(1088) });

            migrationBuilder.CreateIndex(
                name: "IX_CommentAttachments_CommentId",
                table: "CommentAttachments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAttachments_FileID",
                table: "CommentAttachments",
                column: "FileID");
        }
    }
}
