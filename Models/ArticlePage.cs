namespace server.Models
{
    public class ArticlePage
    {
        public int ArticlePageId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string Text { get; set; }
        public int PageNumber{ get; set; }
        public List<ArticlePageComment> ArticlePageComments { get; set; }
        public List<ArticlePageLike> ArticlePageLikes { get; set; }
    }
}
