namespace server.Models
{
    public class ArticleKeyWord
    {
        public int ArticleKeyWordId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int KeyWordId { get; set; }
        public KeyWord KeyWord { get; set; }
    }
}
