using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Chat.Data.Context
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<GameRool> GameRooms { get; set; }
    }
}