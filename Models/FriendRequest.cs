using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class FriendRequest
    {
        [Key]
        public int RequestID { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public string Message { get; set; }
    }
}
