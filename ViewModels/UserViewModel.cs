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
        public string Email { get; set; }
        public string Status { get; set; }
        public DateTime StatusFrom { get; set; }
        public UserViewModel(User user)
        {
            this.UserId = user.UserId;
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.Nickname = user.Nickname;
            this.Login = user.Login;
            this.Role = user.Role.RoleName;
            this.ProfileImage = user.Image.FileLink;
            this.Email=user.Email;
            this.StatusFrom = user?.UserStatus?.StatusFrom ?? DateTime.Now;
            this.Status = user?.UserStatus?.StatusName ?? "";
        }
    }
}
