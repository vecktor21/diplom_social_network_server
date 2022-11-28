using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 28, 20, 7, 58, 404, DateTimeKind.Local).AddTicks(2548),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 26, 16, 46, 59, 35, DateTimeKind.Local).AddTicks(8252));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 28, 20, 7, 58, 445, DateTimeKind.Local).AddTicks(7907));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 28, 20, 7, 58, 445, DateTimeKind.Local).AddTicks(7926));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2022, 11, 28, 20, 7, 58, 445, DateTimeKind.Local).AddTicks(8008));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$HkJAzVxGKPdyaWmEYngV8OlkL./SC3vvcEDQVNnD6fXVR79XoUQnS", new DateTime(2022, 11, 28, 20, 7, 58, 732, DateTimeKind.Local).AddTicks(7859) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Comments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 16, 46, 59, 35, DateTimeKind.Local).AddTicks(8252),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 28, 20, 7, 58, 404, DateTimeKind.Local).AddTicks(2548));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 16, 46, 59, 49, DateTimeKind.Local).AddTicks(4802));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 16, 46, 59, 49, DateTimeKind.Local).AddTicks(4816));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2022, 11, 26, 16, 46, 59, 49, DateTimeKind.Local).AddTicks(4858));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$.NQ1NB3rJ.ILIwnaT3k.kuIDsqNaYQsdczkOhrX2xOudUiWv40Cy.", new DateTime(2022, 11, 26, 16, 46, 59, 263, DateTimeKind.Local).AddTicks(3600) });
        }
    }
}
