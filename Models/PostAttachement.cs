namespace server.Models
{
    public class PostAttachement
    {
        public int PostAttachementId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int FileId { get; set; }
        public File File { get; set; }
    }
}
