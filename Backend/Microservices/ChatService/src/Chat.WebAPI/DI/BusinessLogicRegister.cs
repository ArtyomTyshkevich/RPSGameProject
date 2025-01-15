using Chat.Application.Interfaces;
using Chat.Data.Repositories.UnitOfWork;
using Chat.Data.Repositories;
using FluentValidation.AspNetCore;
using Chat.Data.Services;
using Chat.Data.Configuration;
using Chat.Application.Mappings;

namespace Chat.WebAPI.DI
{
    public static class BusinessLogicRegister
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
            services.AddFluentValidationAutoValidation();
            services.ConfigureDatabase(configuration);
            services.ConfigureCache(configuration);
            services.ConfigureMongoDb(configuration);
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        }
    }
}