namespace server.ViewModels
{
    public class ArticleCreateViewModel
    {
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string? Introduction { get; set; }
        public List<int> KeyWords { get; set; }
    }
}
