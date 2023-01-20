namespace server.ViewModels
{
    public class MessageCreateViewModel
    {
        public int ChatRoomId { get; set; }
        public string Text { get; set; }
        public int SenderId { get; set; }
        public List<int> MessageAttachemntIds { get; set; }
    }
}