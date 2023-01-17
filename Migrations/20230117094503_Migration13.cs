using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 17, 15, 45, 1, 180, DateTimeKind.Local).AddTicks(454),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 11, 18, 21, 12, 842, DateTimeKind.Local).AddTicks(6278));

            migrationBuilder.CreateTable(
                name: "ChatRoomType",
                columns: table => new
                {
                    ChatRoomTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatRoomTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomType", x => x.ChatRoomTypeId);
                });

            migrationBuilder.CreateTable(
                name: "UserChatRoomRole",
                columns: table => new
                {
                    UserChatRoomRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChatRoomRole", x => x.UserChatRoomRoleId);
                });

            migrationBuilder.CreateTable(
                name: "ChatRoom",
                columns: table => new
                {
                    ChatRoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatRoomTypeId = table.Column<int>(type: "int", nullable: false),
                    ChatRoomImageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChatRoomImageFileId = table.Column<int>(type: "int", nullable: false),
                    ChatRoomName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoom", x => x.ChatRoomId);
                    table.ForeignKey(
                        name: "FK_ChatRoom_ChatRoomType_ChatRoomTypeId",
                        column: x => x.ChatRoomTypeId,
                        principalTable: "ChatRoomType",
                        principalColumn: "ChatRoomTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoom_Files_ChatRoomImageFileId",
                        column: x => x.ChatRoomImageFileId,
                        principalTable: "Files",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ChatRoomId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Message_ChatRoom_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRoom",
                        principalColumn: "ChatRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserChatRoom",
                columns: table => new
                {
                    UserChatRoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatRoomId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserChatRoomRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChatRoom", x => x.UserChatRoomId);
                    table.ForeignKey(
                        name: "FK_UserChatRoom_ChatRoom_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRoom",
                        principalColumn: "ChatRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChatRoom_UserChatRoomRole_UserChatRoomRoleId",
                        column: x => x.UserChatRoomRoleId,
                        principalTable: "UserChatRoomRole",
                        principalColumn: "UserChatRoomRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChatRoom_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "ChatRoomType",
                columns: new[] { "ChatRoomTypeId", "ChatRoomTypeName" },
                values: new object[,]
                {
                    { 1, "private" },
                    { 2, "public" }
                });

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 17, 15, 45, 1, 198, DateTimeKind.Local).AddTicks(4031));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 17, 15, 45, 1, 198, DateTimeKind.Local).AddTicks(4043));

            migrationBuilder.InsertData(
                table: "UserChatRoomRole",
                columns: new[] { "UserChatRoomRoleId", "Role" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "user" }
                });

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2023, 1, 17, 15, 45, 1, 198, DateTimeKind.Local).AddTicks(4144));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$uNwaD1Kbqy5KLWzk9HCrAupdwO9cafUBdWdVXEs14aNbzaFeQWIu2", new DateTime(2023, 1, 17, 15, 45, 1, 952, DateTimeKind.Local).AddTicks(5471) });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoom_ChatRoomImageFileId",
                table: "ChatRoom",
                column: "ChatRoomImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoom_ChatRoomTypeId",
                table: "ChatRoom",
                column: "ChatRoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatRoomId",
                table: "Message",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                table: "Message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChatRoom_ChatRoomId",
                table: "UserChatRoom",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChatRoom_UserChatRoomRoleId",
                table: "UserChatRoom",
                column: "UserChatRoomRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChatRoom_UserId",
                table: "UserChatRoom",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "UserChatRoom");

            migrationBuilder.DropTable(
                name: "ChatRoom");

            migrationBuilder.DropTable(
                name: "UserChatRoomRole");

            migrationBuilder.DropTable(
                name: "ChatRoomType");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 11, 18, 21, 12, 842, DateTimeKind.Local).AddTicks(6278),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 17, 15, 45, 1, 180, DateTimeKind.Local).AddTicks(454));

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
    }
}
