namespace server.Models
{
    public class ArticlePageLike
    {
        public int ArticlePageLikeId { get; set; }
        public int ArticlePageId { get; set; }
        public ArticlePage ArticlePage { get; set; }
        public int LikeId { get; set; }
        public Like Like { get; set; }
    }
}
