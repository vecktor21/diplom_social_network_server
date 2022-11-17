using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Subscribe
    {
        public int SubscribeId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SubId { get; set; }
        public User Sub { get; set; }
    }
}
