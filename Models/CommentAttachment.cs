namespace server.Models
{
    public class CommentAttachment
    {
        public int CommentAttachmentId { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public int FileID { get; set; }
        public File File { get; set; }
    }
}
