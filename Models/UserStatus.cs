namespace server.Models
{
    public class UserStatus
    {
        public int UserStatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime StatusFrom { get; set; }
        public User User { get; set; }
    }
}
