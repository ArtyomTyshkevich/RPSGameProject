using Microsoft.AspNetCore.Identity;

namespace Auth.DAL.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Nickname { get; set; } = null!;
        public int Ratting { get; set; } = 0;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}