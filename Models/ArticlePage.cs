namespace server.Models
{
    public class ArticlePage
    {
        //ID страницы
        public int ArticlePageId { get; set; }
        //ссылка на статью
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        //текст на странице
        public string Text { get; set; }
        //датьа публикации
        public DateTime PublicationDate { get; set; }
        //ссылка на комментарии
        public List<ArticlePageComment> ArticlePageComments { get; set; }
        //ссылка на лайки
        public List<ArticlePageLike> ArticlePageLikes { get; set; }
    }
}
