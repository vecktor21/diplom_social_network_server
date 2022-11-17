namespace server.Models
{
    public class PostComment
    {
        public int PostCommentId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
