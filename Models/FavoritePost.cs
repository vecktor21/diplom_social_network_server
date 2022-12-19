namespace server.Models
{
    public class FavoritePost
    {
        public int FavoritePostId { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public Favorite Favorite { get; set; }
        public int FavoriteId { get; set; }
    }
}
