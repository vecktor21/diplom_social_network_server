namespace server.Models
{
    public class UserFile
    {
        public int UserFileId { get; set; }
        public int FileId { get; set; }
        public Models.File File { get; set; }
        public int UserId { get; set; }
        public User User{ get; set; }
    }
}
