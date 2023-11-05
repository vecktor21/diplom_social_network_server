using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class UserToken
    {
        [Key]
        public int TokenId { get; set; }
        public string? RefreshToken { get; set; }
        public User User { get; set; }
    }
}
