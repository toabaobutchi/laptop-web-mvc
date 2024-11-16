using System.ComponentModel.DataAnnotations;

namespace MyLaptopWebsite.Utils
{
    public class UserForm
    {
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [EmailAddress(ErrorMessage = "Hãy nhập một địa chỉ enail hợp lệ")]
        public string Email { get; set; }
    }
}
