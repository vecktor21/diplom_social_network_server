namespace server.Models
{
    public class UserNote
    {
        public int UserNoteId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
