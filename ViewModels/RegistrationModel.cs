

using System.ComponentModel.DataAnnotations;

namespace server.ViewModels
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "введите логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "введите почту")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "введите никнейм")]
        public string Nickname { get; set; }
        [Required(ErrorMessage = "введите имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "введите фамилию")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "повторно введите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "пароли не совпадают")]
        public string ConfirmPassword { get; set; }
        public UserInfoViewModel UserInfo { get; set; }
        public string Role { get; set; } = "USER";
    }
}
