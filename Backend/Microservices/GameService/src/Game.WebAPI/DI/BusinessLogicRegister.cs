using Auth.BLL.Repositories.UnitOfWork;
using Game.Application.Interfaces;
using Game.Data.Repositories;

namespace Game.WebAPI.DI
{
    public static class BusinessLogicRegister
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoundRepository,RoundRepository>();
        }
    }
}
