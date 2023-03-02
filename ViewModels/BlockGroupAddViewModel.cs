namespace server.ViewModels
{
    public class BlockGroupAddViewModel
    {
        public int BlockedUserId { get; set; }
        public int GroupId { get; set; }    
        public string? Reason { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
