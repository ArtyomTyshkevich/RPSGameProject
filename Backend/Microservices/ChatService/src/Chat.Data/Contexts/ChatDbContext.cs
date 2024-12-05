using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace Chat.Data.Context
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }
        public DbSet<User> Users { get; set; }
    }
}