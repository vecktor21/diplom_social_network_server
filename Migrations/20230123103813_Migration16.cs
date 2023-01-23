using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SendingTime",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 23, 16, 38, 12, 649, DateTimeKind.Local).AddTicks(3254),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 18, 11, 51, 11, 93, DateTimeKind.Local).AddTicks(8566));

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendingTime",
                table: "Messages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 18, 11, 51, 11, 93, DateTimeKind.Local).AddTicks(8566),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 23, 16, 38, 12, 649, DateTimeKind.Local).AddTicks(3254));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 18, 11, 51, 11, 109, DateTimeKind.Local).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 18, 11, 51, 11, 109, DateTimeKind.Local).AddTicks(4856));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2023, 1, 18, 11, 51, 11, 109, DateTimeKind.Local).AddTicks(4913));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$lPCtJTR3sIeAQ0WC8ZPohuO9IND.IWZr6vU2/WcNZ1CaJnzL5ZSJW", new DateTime(2023, 1, 18, 11, 51, 11, 372, DateTimeKind.Local).AddTicks(1073) });
        }
    }
}
