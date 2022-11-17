using server.Models;

namespace server.ViewModels
{
    public class GroupMemberViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public string ProfileImage { get; set; }
        public GroupMemberRole GroupMemberRole { get; set; }
        public GroupMemberViewModel(User user, GroupMemberRole role)
        {
            this.UserId = user.UserId;
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.Nickname = user.Nickname;
            this.ProfileImage = user.Image.FileLink;
            this.GroupMemberRole = role;
        }
    }
}
