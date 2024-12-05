using Auth.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.DAL.DI
{
    public static class DatabaseSetup
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}