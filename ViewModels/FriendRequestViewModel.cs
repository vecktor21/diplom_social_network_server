using server.Models;

namespace server.ViewModels
{
    public class FriendRequestViewModel
    {
        public int UserId { get; set; }
        public int SenderId { get; set; }
        public string Message { get; set; }
    }
}
