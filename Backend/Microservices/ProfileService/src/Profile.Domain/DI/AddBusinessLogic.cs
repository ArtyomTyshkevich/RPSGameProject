using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.BLL.Mappings;
using Profile.DAL.DI;

namespace Profile.BLL.DI
{
    public static class BusinessLogicRegister
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddScoped<IRoomService, RoomService>();
            //services.AddScoped<IRoundService, RoundService>();
            //services.AddScoped<ISearchService, SearchService>();
            //services.AddScoped<ICacheService, CacheService>();
            //services.ConfigureMassTransit(configuration);
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddFluentValidationAutoValidation();
        }
    }
}