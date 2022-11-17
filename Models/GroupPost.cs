namespace server.Models
{
    public class GroupPost
    {
        public int GroupPostId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
