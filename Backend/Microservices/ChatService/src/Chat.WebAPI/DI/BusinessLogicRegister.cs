using Chat.Application.Interfaces;
using Chat.Data.Repositories.UnitOfWork;
using Chat.Data.Repositories;
using FluentValidation.AspNetCore;
using Chat.WebAPI.Configs;
using Chat.Data.Services;

namespace Chat.WebAPI.DI
{
    public static class BusinessLogicRegister
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidationAutoValidation();
            services.ConfigureDatabase(configuration);
            services.ConfigureCache(configuration);
            services.ConfigureMongoDb(configuration);
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}