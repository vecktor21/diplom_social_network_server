namespace server.ViewModels
{
    public class ChatRoomCreateViewModel
    {
        public string ChatRoomName { get; set; }
        public int ChatRoomTypeId { get; set; }
        public List<int> ChatRoomMembers { get; set; }
        public int AdminId { get; set; }
    }
}
