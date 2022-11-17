using System.ComponentModel.DataAnnotations;

namespace server.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "не ввели логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
