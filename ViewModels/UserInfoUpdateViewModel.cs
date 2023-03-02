using server.Models;

namespace server.ViewModels
{
    public class UserInfoUpdateViewModel
    {
        public int UserInfoId { get; set; }
        public int UserId { get; set; }
        public int? Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Country Country { get; set; } = null!;
        public string? City { get; set; }
        public string? Status { get; set; }
        public string? Education { get; set; }
        public UserInfoPrivacyType UserInfoPrivacyType { get; set; } = null!;
    }
}
