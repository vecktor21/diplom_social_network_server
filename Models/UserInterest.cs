namespace server.Models
{
    public class UserInterest
    {
        public int UserInterestId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int KeyWordId { get; set; }
        public KeyWord KeyWord { get; set; }
    }
}
