using server.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class GroupBlockList : IBlockList
    {
        public int GroupBlockListId { get; set; }
        public int BlockedUserId { get; set; }
        public User BlockedUser { get; set; } = null!;
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public string? Reason { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
