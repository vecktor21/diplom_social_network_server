namespace server.Models
{
    public class ArticlePageComment
    {
        public int ArticlePageCommentId { get; set; }
        public int ArticlePageId { get; set; }
        public ArticlePage ArticlePage { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
