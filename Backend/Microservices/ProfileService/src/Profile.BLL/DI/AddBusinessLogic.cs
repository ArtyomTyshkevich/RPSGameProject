using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.BLL.Configuretion;
using Profile.BLL.Interfaces.Repositories;
using Profile.BLL.Interfaces.Services;
using Profile.BLL.Mappings;
using Profile.BLL.Repositories;
using Profile.BLL.Repositories.UnitOfWork;
using Profile.BLL.Services;
using Profile.DAL.DI;

namespace Profile.BLL.DI
{
    public static class BusinessLogicRegister
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IUserService, UserService>();
            services.ConfigureBrokerMassTransit(configuration);
            services.AddScoped<IGlobalStatisticsServices, GlobalStatisticsServices>();
            services.AddScoped<IUserStatisticsService, UserStatisticsService>();
            services.ConfigureDatabase(configuration);
            services.Configure<MongoDbConfiguration>
                (
                configuration.GetSection("MongoDbSettings")
                );
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddFluentValidationAutoValidation();
        }
    }
}