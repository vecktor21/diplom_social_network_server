using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 15, 26, 4, 457, DateTimeKind.Local).AddTicks(9322),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 26, 15, 16, 25, 853, DateTimeKind.Local).AddTicks(9068));

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 15, 16, 25, 853, DateTimeKind.Local).AddTicks(9068),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 26, 15, 26, 4, 457, DateTimeKind.Local).AddTicks(9322));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 15, 16, 25, 879, DateTimeKind.Local).AddTicks(5538));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 15, 16, 25, 879, DateTimeKind.Local).AddTicks(5555));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2022, 11, 26, 15, 16, 25, 879, DateTimeKind.Local).AddTicks(5646));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$bKFKtqKuUmw3S8M33QkpU.0Im63bPQpQqtHNf12fOS3zssX.BozMC", new DateTime(2022, 11, 26, 15, 16, 26, 228, DateTimeKind.Local).AddTicks(1681) });
        }
    }
}
