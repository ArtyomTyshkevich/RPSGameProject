using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Auth.DAL.Entities;

namespace Auth.DAL.Data
{
    public class AuthDbContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

    }
}