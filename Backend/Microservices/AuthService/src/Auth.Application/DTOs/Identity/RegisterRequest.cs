using System.ComponentModel.DataAnnotations;

namespace Auth.BLL.DTOs.Identity
{
    public class RegisterRequest
    {
        public string Email { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = null!;
        public string PasswordConfirm { get; set; } = null!;
        public string Nickname { get; set; } = null!;
    }
}