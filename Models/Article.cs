namespace server.Models
{
    public class Article
    {
        //ID статьи
        public int ArticleId { get; set; }
        //автор
        public int AuthorId { get; set; }
        public User Author { get; set; }
        //название статьи
        public string Title { get; set; }
        //введение в статью
        public string Introduction { get; set; }
        //датьа публикации
        public DateTime PublicationDate { get; set; }
        //ключевые слова статьи
        public List<ArticleKeyWord> ArticleKeyWords { get; set; }
        //ссылки на страницы статьи
        public List<ArticlePage> ArticlePages { get; set; }
        //комментарии
        public List<ArticleComment> ArticleComments { get; set; }
        //лайки
        public List<ArticleLike> ArticleLikes { get; set; }

    }
}
