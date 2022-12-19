using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Articles_ArticleId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_ArticleId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Favorites");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 19, 21, 1, 0, 121, DateTimeKind.Local).AddTicks(9998),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 12, 19, 20, 57, 17, 348, DateTimeKind.Local).AddTicks(5240));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 19, 20, 57, 17, 348, DateTimeKind.Local).AddTicks(5240),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 12, 19, 21, 1, 0, 121, DateTimeKind.Local).AddTicks(9998));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2022, 12, 19, 20, 57, 17, 367, DateTimeKind.Local).AddTicks(1382));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2022, 12, 19, 20, 57, 17, 367, DateTimeKind.Local).AddTicks(1404));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2022, 12, 19, 20, 57, 17, 367, DateTimeKind.Local).AddTicks(1463));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$xHPNm2nHCBpv8RvEAkZt7u37ZFiifobA0s7jS9L9TX3kvyGHhflpa", new DateTime(2022, 12, 19, 20, 57, 17, 681, DateTimeKind.Local).AddTicks(9998) });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ArticleId",
                table: "Favorites",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Articles_ArticleId",
                table: "Favorites",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId");
        }
    }
}
