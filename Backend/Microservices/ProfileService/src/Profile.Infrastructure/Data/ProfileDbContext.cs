using Microsoft.EntityFrameworkCore;
using Profile.DAL.Entities;

namespace Profile.DAL.Data
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<User> Users { get; set; }
    }
}