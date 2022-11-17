using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Friend
    {
        public int FriendId { get; set; }
        
        public int User1Id { get; set; }
        public User User1{ get; set; }
        public int User2Id { get; set; }
        public User User2 { get; set; }
    }
}
