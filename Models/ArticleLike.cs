namespace server.Models
{
    public class ArticleLike
    {
        public int ArticleLikeId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int LikeId { get; set; }
        public Like Like { get; set; }
    }
}
