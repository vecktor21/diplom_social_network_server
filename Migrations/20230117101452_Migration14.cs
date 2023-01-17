using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Migration14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoom_ChatRoomType_ChatRoomTypeId",
                table: "ChatRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoom_Files_ChatRoomImageFileId",
                table: "ChatRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_ChatRoom_ChatRoomId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_UserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChatRoom_ChatRoom_ChatRoomId",
                table: "UserChatRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChatRoom_UserChatRoomRole_UserChatRoomRoleId",
                table: "UserChatRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChatRoom_Users_UserId",
                table: "UserChatRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChatRoomRole",
                table: "UserChatRoomRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChatRoom",
                table: "UserChatRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatRoomType",
                table: "ChatRoomType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatRoom",
                table: "ChatRoom");

            migrationBuilder.RenameTable(
                name: "UserChatRoomRole",
                newName: "UserChatRoomRoles");

            migrationBuilder.RenameTable(
                name: "UserChatRoom",
                newName: "UserChatRooms");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "ChatRoomType",
                newName: "ChatRoomTypes");

            migrationBuilder.RenameTable(
                name: "ChatRoom",
                newName: "ChatRooms");

            migrationBuilder.RenameIndex(
                name: "IX_UserChatRoom_UserId",
                table: "UserChatRooms",
                newName: "IX_UserChatRooms_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChatRoom_UserChatRoomRoleId",
                table: "UserChatRooms",
                newName: "IX_UserChatRooms_UserChatRoomRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChatRoom_ChatRoomId",
                table: "UserChatRooms",
                newName: "IX_UserChatRooms_ChatRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserId",
                table: "Messages",
                newName: "IX_Messages_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ChatRoomId",
                table: "Messages",
                newName: "IX_Messages_ChatRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoom_ChatRoomTypeId",
                table: "ChatRooms",
                newName: "IX_ChatRooms_ChatRoomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoom_ChatRoomImageFileId",
                table: "ChatRooms",
                newName: "IX_ChatRooms_ChatRoomImageFileId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 17, 16, 14, 51, 135, DateTimeKind.Local).AddTicks(2769),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 17, 15, 45, 1, 180, DateTimeKind.Local).AddTicks(454));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChatRoomRoles",
                table: "UserChatRoomRoles",
                column: "UserChatRoomRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChatRooms",
                table: "UserChatRooms",
                column: "UserChatRoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatRoomTypes",
                table: "ChatRoomTypes",
                column: "ChatRoomTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatRooms",
                table: "ChatRooms",
                column: "ChatRoomId");

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 17, 16, 14, 51, 154, DateTimeKind.Local).AddTicks(1082));

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileId",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2023, 1, 17, 16, 14, 51, 154, DateTimeKind.Local).AddTicks(1096));

            migrationBuilder.UpdateData(
                table: "UserStatuses",
                keyColumn: "UserStatusId",
                keyValue: 1,
                column: "StatusFrom",
                value: new DateTime(2023, 1, 17, 16, 14, 51, 154, DateTimeKind.Local).AddTicks(1187));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "RegistrationDate" },
                values: new object[] { "$2a$11$wcbQsoEWq3BZLb9w4LF/TeB/xjQfFlGi6oMyIosD1y1MQOsk0Y062", new DateTime(2023, 1, 17, 16, 14, 51, 416, DateTimeKind.Local).AddTicks(2879) });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_ChatRoomTypes_ChatRoomTypeId",
                table: "ChatRooms",
                column: "ChatRoomTypeId",
                principalTable: "ChatRoomTypes",
                principalColumn: "ChatRoomTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Files_ChatRoomImageFileId",
                table: "ChatRooms",
                column: "ChatRoomImageFileId",
                principalTable: "Files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoomId",
                table: "Messages",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatRooms_ChatRooms_ChatRoomId",
                table: "UserChatRooms",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatRooms_UserChatRoomRoles_UserChatRoomRoleId",
                table: "UserChatRooms",
                column: "UserChatRoomRoleId",
                principalTable: "UserChatRoomRoles",
                principalColumn: "UserChatRoomRoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatRooms_Users_UserId",
                table: "UserChatRooms",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_ChatRoomTypes_ChatRoomTypeId",
                table: "ChatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_Files_ChatRoomImageFileId",
                table: "ChatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoomId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChatRooms_ChatRooms_ChatRoomId",
                table: "UserChatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChatRooms_UserChatRoomRoles_UserChatRoomRoleId",
                table: "UserChatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChatRooms_Users_UserId",
                table: "UserChatRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChatRooms",
                table: "UserChatRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChatRoomRoles",
                table: "UserChatRoomRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatRoomTypes",
                table: "ChatRoomTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatRooms",
                table: "ChatRooms");

            migrationBuilder.RenameTable(
                name: "UserChatRooms",
                newName: "UserChatRoom");

            migrationBuilder.RenameTable(
                name: "UserChatRoomRoles",
                newName: "UserChatRoomRole");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameTable(
                name: "ChatRoomTypes",
                newName: "ChatRoomType");

            migrationBuilder.RenameTable(
                name: "ChatRooms",
                newName: "ChatRoom");

            migrationBuilder.RenameIndex(
                name: "IX_UserChatRooms_UserId",
                table: "UserChatRoom",
                newName: "IX_UserChatRoom_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChatRooms_UserChatRoomRoleId",
                table: "UserChatRoom",
                newName: "IX_UserChatRoom_UserChatRoomRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChatRooms_ChatRoomId",
                table: "UserChatRoom",
                newName: "IX_UserChatRoom_ChatRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserId",
                table: "Message",
                newName: "IX_Message_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChatRoomId",
                table: "Message",
                newName: "IX_Message_ChatRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRooms_ChatRoomTypeId",
                table: "ChatRoom",
                newName: "IX_ChatRoom_ChatRoomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRooms_ChatRoomImageFileId",
                table: "ChatRoom",
                newName: "IX_ChatRoom_ChatRoomImageFileId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "BlockList",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 17, 15, 45, 1, 180, DateTimeKind.Local).AddTicks(454),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 1, 17, 16, 14, 51, 135, DateTimeKind.Local).AddTicks(2769));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChatRoom",
                table: "UserChatRoom",
                column: "UserChatRoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChatRoomRole",
                table: "UserChatRoomRole",
                column: "UserChatRoomRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatRoomType",
                table: "ChatRoomType",
                column: "ChatRoomTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatRoom",
                table: "ChatRoom",
                column: "ChatRoomId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoom_ChatRoomType_ChatRoomTypeId",
                table: "ChatRoom",
                column: "ChatRoomTypeId",
                principalTable: "ChatRoomType",
                principalColumn: "ChatRoomTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoom_Files_ChatRoomImageFileId",
                table: "ChatRoom",
                column: "ChatRoomImageFileId",
                principalTable: "Files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_ChatRoom_ChatRoomId",
                table: "Message",
                column: "ChatRoomId",
                principalTable: "ChatRoom",
                principalColumn: "ChatRoomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_UserId",
                table: "Message",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatRoom_ChatRoom_ChatRoomId",
                table: "UserChatRoom",
                column: "ChatRoomId",
                principalTable: "ChatRoom",
                principalColumn: "ChatRoomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatRoom_UserChatRoomRole_UserChatRoomRoleId",
                table: "UserChatRoom",
                column: "UserChatRoomRoleId",
                principalTable: "UserChatRoomRole",
                principalColumn: "UserChatRoomRoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatRoom_Users_UserId",
                table: "UserChatRoom",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
