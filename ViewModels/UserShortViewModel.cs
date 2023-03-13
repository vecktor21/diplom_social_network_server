using server.Models;

namespace server.ViewModels
{
    public class UserShortViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public int? GroupMemberRole { get; set; }
        public UserShortViewModel(User user, GroupMemberRole role)
        {
            this.UserId = user.UserId;
            this.FullName = user.Name + " " + user.Surname;
            this.ProfileImage = user.Image.FileLink;
            this.GroupMemberRole = role.GroupMemberRoleId;
        }
        public UserShortViewModel(User user)
        {
            this.UserId = user.UserId;
            this.FullName = user.Name + " " + user.Surname;
            this.ProfileImage = user.Image.FileLink;
        }
        public UserShortViewModel(int userId, string fullName, string profileImage)
        {
            this.UserId = userId;
            this.FullName = fullName;
            this.ProfileImage = profileImage;
        }
    }
}
