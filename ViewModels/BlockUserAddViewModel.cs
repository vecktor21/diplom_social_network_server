namespace server.ViewModels
{
    public class BlockUserAddViewModel
    {
        public int BlockedUserId { get; set; }
        public int UserId { get; set; }
        public string? Reason { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
