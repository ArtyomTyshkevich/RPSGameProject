using Auth.BLL.Repositories.UnitOfWork;
using Chat.Data.Extensions;
using Chat.Data.Services;
using FluentValidation.AspNetCore;
using Game.Application.Interfaces.Repositories;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Application.Mappers;
using Game.Data.HangfireJobs;
using Game.Data.Repositories;
using Game.Data.Services;
using Game.WebAPI.Setups;

namespace Game.WebAPI.DI
{
    public static class BusinessLogicRegister
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IGameRuleRepository, GameRuleRepository>();
            services.AddScoped<IRoundRepository,RoundRepository>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoundService, RoundService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<CleanRoomsJob>();
            services.ConfigureMassTransit(configuration);
            services.ConfigureBrokerMassTransit(configuration);
            services.ConfigureDatabase(configuration);
            services.ConfigureCache(configuration);
            services.ConfigureAuthentication(configuration);
            services.AddHangfireConfig(configuration);
            services.ConfigureSwagger();
            services.AddAutoMapper(typeof(RoomMappingProfile).Assembly);
            services.AddFluentValidationAutoValidation();
        }
    }
}
