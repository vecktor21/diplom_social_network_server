using server.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class UserBlockList : IBlockList
    {
        public int UserBlockListId { get; set; }
        public int BlockedUserId { get; set; }
        public User BlockedUser { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string? Reason { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
