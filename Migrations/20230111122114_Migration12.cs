using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 11, 18, 21, 12, 842, DateTimeKind.Local).AddTicks(6278),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 11, 18, 15, 30, 954, DateTimeKind.Local).AddTicks(9344));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "ArticlePages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 11, 18, 21, 12, 857, DateTimeKind.Local).AddTicks(9220));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 11, 18, 21, 12, 857, DateTimeKind.Local).AddTicks(9229));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2023, 1, 11, 18, 21, 12, 857, DateTimeKind.Local).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$Iy1uxzRBvirCodbxc4zju.9s8tcUmz34MQ5mjP4CbxTLxhZhgN16m", new DateTime(2023, 1, 11, 18, 21, 13, 65, DateTimeKind.Local).AddTicks(3667) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 11, 18, 15, 30, 954, DateTimeKind.Local).AddTicks(9344),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 11, 18, 21, 12, 842, DateTimeKind.Local).AddTicks(6278));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "ArticlePages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 11, 18, 15, 30, 969, DateTimeKind.Local).AddTicks(1707));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 11, 18, 15, 30, 969, DateTimeKind.Local).AddTicks(1717));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2023, 1, 11, 18, 15, 30, 969, DateTimeKind.Local).AddTicks(1759));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$8YdqiHuegWOKmTX3KrqkpuZABAK95WvtCfDgZv1XR6.j4oPu35fRW", new DateTime(2023, 1, 11, 18, 15, 31, 127, DateTimeKind.Local).AddTicks(4023) });
        }
    }
}
