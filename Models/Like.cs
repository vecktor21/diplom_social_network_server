using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public int LikedUserId { get; set; }
        public User LikedUser { get; set; }
    }
}
