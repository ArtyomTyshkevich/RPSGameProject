using Chat.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Chat.WebAPI.Configs
{
    public static class MongoDbSetup
    {
        public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}