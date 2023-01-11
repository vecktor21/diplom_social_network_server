using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "ArticlePages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleLikes");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "ArticlePages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 20, 21, 45, 11, 284, DateTimeKind.Local).AddTicks(7249),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 11, 18, 15, 30, 954, DateTimeKind.Local).AddTicks(9344));

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "ArticlePageLikes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArticleLikeId",
                table: "ArticlePageLikes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

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

            migrationBuilder.CreateIndex(
                name: "IX_ArticlePageLikes_ArticleId",
                table: "ArticlePageLikes",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticlePageLikes_Articles_ArticleId",
                table: "ArticlePageLikes",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
