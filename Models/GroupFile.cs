namespace server.Models
{
    public class GroupFile
    {
        public int GroupFileId { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int FileId { get; set; }
        public File File { get; set; }
        
    }
}
