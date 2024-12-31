using Chat.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Game.WebAPI.Setups
{
    public static class DatabaseSetup
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GameDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!));
        }
    }
}