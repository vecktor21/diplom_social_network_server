using server.ViewModels;

namespace server.Models
{
    public class UserInfo
    {
        public int UserInfoId { get; set; }
        public int? Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public string? City { get; set; }
        public string? Status { get; set; }
        public string? Education { get; set; }
        public int UserInfoPrivacyTypeId { get; set; }
        public UserInfoPrivacyType UserInfoPrivacyType { get; set; }
        public User User { get; set; }
    }
}
