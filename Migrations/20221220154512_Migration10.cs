using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PageNumber",
                table: "ArticlePages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 20, 21, 45, 11, 284, DateTimeKind.Local).AddTicks(7249),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 12, 19, 21, 1, 0, 121, DateTimeKind.Local).AddTicks(9998));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2022, 12, 20, 21, 45, 11, 296, DateTimeKind.Local).AddTicks(4080));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2022, 12, 20, 21, 45, 11, 296, DateTimeKind.Local).AddTicks(4091));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2022, 12, 20, 21, 45, 11, 296, DateTimeKind.Local).AddTicks(4128));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$.64odWFYj1YMqj0EAl1/7eXIfnxZfi1eSt.Xmg6DhMGi1Nb6AL04m", new DateTime(2022, 12, 20, 21, 45, 11, 459, DateTimeKind.Local).AddTicks(1360) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 19, 21, 1, 0, 121, DateTimeKind.Local).AddTicks(9998),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 12, 20, 21, 45, 11, 284, DateTimeKind.Local).AddTicks(7249));

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PageNumber",
                table: "ArticlePages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2022, 12, 19, 21, 1, 0, 149, DateTimeKind.Local).AddTicks(9202));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2022, 12, 19, 21, 1, 0, 149, DateTimeKind.Local).AddTicks(9221));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2022, 12, 19, 21, 1, 0, 149, DateTimeKind.Local).AddTicks(9298));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$RnBfGjvqKA0toYfoQNGw7OAurJ36uN1o0dw6/GPEt978ppCMbbRU2", new DateTime(2022, 12, 19, 21, 1, 0, 440, DateTimeKind.Local).AddTicks(8683) });
        }
    }
}
