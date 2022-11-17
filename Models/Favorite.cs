using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public int ObjectId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("ObjectId")]
        public Post Post { get; set; }
        [ForeignKey("ObjectId")]
        public Article Article { get; set; }
        [ForeignKey("ObjectId")]
        public Group Group { get; set; }
    }
}
