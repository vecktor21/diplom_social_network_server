using server.Models;

namespace server.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public string ProfileImage { get; set; }
        public UserViewModel(User user)
        {
            this.UserId = user.UserId;
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.Nickname = user.Nickname;
            this.Login = user.Login;
            this.Role = user.Role.RoleName;
            this.ProfileImage = user.Image.FileLink;
        }
    }
}
