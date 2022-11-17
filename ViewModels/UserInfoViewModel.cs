using server.Models;

namespace server.ViewModels
{
    public class UserInfoViewModel
    {
        public int? Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Country? Country { get; set; }
        public string? City { get; set; }
        public string? Status { get; set; }
        public string? Education { get; set; }
        public UserInfoPrivacyType UserInfoPrivacyType { get; set; }
}
}
