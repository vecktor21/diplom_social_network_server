namespace server.Models
{
    public class FavoriteArticle
    {
        public int FavoriteArticleId { get; set; }
        public Article Article { get; set; }
        public int ArticleId { get; set; }
        public Favorite Favorite { get; set; }
        public int FavoriteId { get; set; }
    }
}
