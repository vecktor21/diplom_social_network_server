namespace server.ViewModels
{
    public class PostCreateViewModel
    {
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<int> Attachments { get; set; }
    }
}
