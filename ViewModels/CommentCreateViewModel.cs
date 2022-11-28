namespace server.ViewModels
{
    public class CommentCreateViewModel
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Message { get;set; }
        public List<int> AttachmentsId { get; set; }
    }
}
