using FluentValidation.AspNetCore;
using Game.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.BLL.Interfaces.Repositories;
using Profile.BLL.Mappings;
using Profile.BLL.Repositories.UnitOfWork;
using Profile.DAL.DI;

namespace Profile.BLL.DI
{
    public static class BusinessLogicRegister
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IRoundService, RoundService>();
            //services.AddScoped<ISearchService, SearchService>();
            //services.AddScoped<ICacheService, CacheService>();
            //services.ConfigureMassTransit(configuration);
            services.ConfigureMongoDb(configuration);
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddFluentValidationAutoValidation();
        }
    }
}