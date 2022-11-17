namespace server.Models
{
    public class CommentLike
    {
        public int CommentLikeId { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public int LikeId { get; set; }
        public Like Like { get; set; }
    }
}
