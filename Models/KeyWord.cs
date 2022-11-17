namespace server.Models
{
    public class KeyWord
    {
        public int KeyWordId { get; set; }
        public string KeyWordRu { get; set; }
        public string KeyWordEn { get; set; }
        public List<ArticleKeyWord> ArticleKeyWords { get; set; }
        public List<UserInterest> UserInterests { get; set; }
    }
}
