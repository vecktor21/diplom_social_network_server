namespace server.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
        public string Text { get; set; }
        public List<MessageAttachment> MessageAttachments { get; set; }
    }
}
