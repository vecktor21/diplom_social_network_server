namespace server.Models
{
    public class MessageAttachment
    {
        public int MessageAttachmentId { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }
        public int FileId { get; set; }
        public File File { get; set; }
    }
}
