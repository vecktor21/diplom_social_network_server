namespace server.Models
{
    public class FavoriteGroup
    {
        public int FavoriteGroupId { get; set; }
        public Group Group { get; set; }
        public int GroupId { get; set; }
        public Favorite Favorite { get; set; }
        public int FavoriteId { get; set; }
    }
}
