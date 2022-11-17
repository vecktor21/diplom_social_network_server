namespace server.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public string Title { get; set; }
        public string Introduction { get; set; }
        public int Rating { get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<ArticleKeyWord> ArticleKeyWords { get; set; }
        public List<ArticlePage> ArticlePages { get; set; }
        public List<ArticleComment> ArticleComments { get; set; }
        public List<ArticleLike> ArticleLikes { get; set; }

    }
}
