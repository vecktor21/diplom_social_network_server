namespace server.Models
{
    public class PostLike
    {
        public int PostLikeId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int LikeId { get; set; }
        public Like Like { get; set; }
    }
}
