namespace server.Models
{
    public class ReplyComment
    {
        public int ReplyCommentId { get; set; }
        public int MajorCommentId { get; set; }
        public Comment MajorComment { get; set; }
        public int RepliedCommentId { get; set; }
        public Comment RepliedComment { get; set; }
    }
}
