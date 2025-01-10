using Auth.BLL.Interfaces;
using Auth.BLL.Mappers;
using Auth.BLL.Repositories.UnitOfWork;
using Auth.DAL.DI;
using FluentValidation.AspNetCore;
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
            services.AddFluentValidationAutoValidation();
            services.ConfigureAuthentication(configuration);
            services.ConfigureSwagger();
            services.ConfigureDatabase(configuration);
            services.ConfigureGrpcClients(configuration);
            services.ConfigureIdentity();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(UserMappingProfile));
        }
    }
}
