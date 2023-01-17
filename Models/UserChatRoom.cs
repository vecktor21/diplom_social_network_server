namespace server.Models
{
    public class UserChatRoom
    {
        public int UserChatRoomId { get; set; }
        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int UserChatRoomRoleId { get; set; }
        public UserChatRoomRole UserChatRoomRole { get; set; }
    }

}
