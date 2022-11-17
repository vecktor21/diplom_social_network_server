namespace server.Models
{
    public class RequestToGroup
    {
        public int RequestToGroupId { get; set; }
        public int GroupId { get; set; }
        public Group Group { get;set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
