namespace server.Models
{
    public class ArticleComment
    {
        public int ArticleCommentId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
