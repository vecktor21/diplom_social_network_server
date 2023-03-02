using server.Models;

namespace server.ViewModels
{
    public interface IBlockList
    {
        public int BlockedUserId { get; set; }
        public User BlockedUser { get; set; }
        public string? Reason { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
