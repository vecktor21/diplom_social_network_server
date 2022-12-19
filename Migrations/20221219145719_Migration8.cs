using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Articles_ObjectId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Groups_ObjectId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Posts_ObjectId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_ObjectId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "Favorites");

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
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
                oldDefaultValue: new DateTime(2022, 11, 28, 20, 7, 58, 404, DateTimeKind.Local).AddTicks(2548));

            migrationBuilder.CreateTable(
                name: "FavoriteArticles",
                columns: table => new
                {
                    FavoriteArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteArticles", x => x.FavoriteArticleId);
                    table.ForeignKey(
                        name: "FK_FavoriteArticles_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteArticles_Favorites_FavoriteId",
                        column: x => x.FavoriteId,
                        principalTable: "Favorites",
                        principalColumn: "FavoriteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteGroups",
                columns: table => new
                {
                    FavoriteGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteGroups", x => x.FavoriteGroupId);
                    table.ForeignKey(
                        name: "FK_FavoriteGroups_Favorites_FavoriteId",
                        column: x => x.FavoriteId,
                        principalTable: "Favorites",
                        principalColumn: "FavoriteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoritePosts",
                columns: table => new
                {
                    FavoritePostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritePosts", x => x.FavoritePostId);
                    table.ForeignKey(
                        name: "FK_FavoritePosts_Favorites_FavoriteId",
                        column: x => x.FavoriteId,
                        principalTable: "Favorites",
                        principalColumn: "FavoriteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritePosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_GroupId",
                table: "Favorites",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PostId",
                table: "Favorites",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteArticles_ArticleId",
                table: "FavoriteArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteArticles_FavoriteId",
                table: "FavoriteArticles",
                column: "FavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteGroups_FavoriteId",
                table: "FavoriteGroups",
                column: "FavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteGroups_GroupId",
                table: "FavoriteGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePosts_FavoriteId",
                table: "FavoritePosts",
                column: "FavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePosts_PostId",
                table: "FavoritePosts",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Articles_ArticleId",
                table: "Favorites",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Groups_GroupId",
                table: "Favorites",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Posts_PostId",
                table: "Favorites",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Articles_ArticleId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Groups_GroupId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Posts_PostId",
                table: "Favorites");

            migrationBuilder.DropTable(
                name: "FavoriteArticles");

            migrationBuilder.DropTable(
                name: "FavoriteGroups");

            migrationBuilder.DropTable(
                name: "FavoritePosts");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_ArticleId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_GroupId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_PostId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Favorites");

            migrationBuilder.AddColumn<int>(
                name: "ObjectId",
                table: "Favorites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 28, 20, 7, 58, 404, DateTimeKind.Local).AddTicks(2548),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 12, 19, 20, 57, 17, 348, DateTimeKind.Local).AddTicks(5240));

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

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ObjectId",
                table: "Favorites",
                column: "ObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Articles_ObjectId",
                table: "Favorites",
                column: "ObjectId",
                principalTable: "Articles",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Groups_ObjectId",
                table: "Favorites",
                column: "ObjectId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Posts_ObjectId",
                table: "Favorites",
                column: "ObjectId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
