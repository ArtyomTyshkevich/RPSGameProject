

using Game.Data.Configuration;
using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.Context
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<GameRule> GameRools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RulesConfiguration());
        }
    }
}   