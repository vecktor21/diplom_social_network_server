namespace server.Models
{
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        public int ChatRoomTypeId { get; set; }
        public ChatRoomType ChatRoomType { get; set; }
        public string ChatRoomImageId { get; set; }
        public File ChatRoomImage { get; set; }
        public string ChatRoomName { get; set;}
        public List<Message> Messages { get; set; }
        public List<UserChatRoom> UserChatRooms { get; set; }
    }
}
