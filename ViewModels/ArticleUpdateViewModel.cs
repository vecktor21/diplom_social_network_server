namespace server.ViewModels
{
    public class ArticleUpdateViewModel
    {
        //ID статьи
        public int ArticleId { get; set; }
        //название статьи
        public string Title { get; set; }
        //введение в статью
        public string Introduction { get; set; }
        //ключевые слова
        public List<int> KeyWords { get; set; }
    }
}
