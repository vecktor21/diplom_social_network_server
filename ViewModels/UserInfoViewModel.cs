using server.Models;

namespace server.ViewModels
{
    public class UserInfoViewModel
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

        public UserInfoViewModel(User user)
        {
            this.UserInfoId = user.UserInfo.UserInfoId;
            this.UserId = user.UserId;
            this.Age = user.UserInfo.Age;
            this.DateOfBirth = user.UserInfo.DateOfBirth;
            this.Country = user.UserInfo.Country;
            this.City = user.UserInfo.City;
            this.Status = user.UserInfo.Status;
            this.Education = user.UserInfo.Education;
            this.UserInfoPrivacyType = user.UserInfo.UserInfoPrivacyType;
        }
        public UserInfoViewModel()
        {

        }

    }
}
