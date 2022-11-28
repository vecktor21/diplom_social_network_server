using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostAttachementId",
                table: "PostAttachments",
                newName: "PostAttachmentId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 15, 16, 25, 853, DateTimeKind.Local).AddTicks(9068),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 26, 14, 59, 30, 86, DateTimeKind.Local).AddTicks(1944));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostAttachmentId",
                table: "PostAttachments",
                newName: "PostAttachementId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 14, 59, 30, 86, DateTimeKind.Local).AddTicks(1944),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 11, 26, 15, 16, 25, 853, DateTimeKind.Local).AddTicks(9068));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 14, 59, 30, 120, DateTimeKind.Local).AddTicks(5799));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2022, 11, 26, 14, 59, 30, 120, DateTimeKind.Local).AddTicks(5813));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2022, 11, 26, 14, 59, 30, 120, DateTimeKind.Local).AddTicks(5870));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$13MtDIA1x/nc7.2Bpj3w9.blCUBajzjgoh09meTmokhT2Us2TQytO", new DateTime(2022, 11, 26, 14, 59, 30, 392, DateTimeKind.Local).AddTicks(3548) });
        }
    }
}
