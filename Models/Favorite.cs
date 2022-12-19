using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
