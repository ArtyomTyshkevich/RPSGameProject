using Auth.BLL.Interfaces;
using Auth.BLL.Repositories.UnitOfWork;
using Auth.DAL.DI;
using Library.Application.Interfaces;
using Library.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.BLL.DI
{
    public static class BusinessLogicRegister
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAuthentication(configuration);
            services.ConfigureDatabase(configuration);
            services.ConfigureIdentity();
            services.ConfigureSwagger();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
