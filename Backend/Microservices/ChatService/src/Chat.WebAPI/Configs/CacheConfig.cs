﻿
namespace Chat.WebAPI.Configs
{
    public static class CacheSetup
    {
        public static void ConfigureCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(option =>
                option.Configuration = configuration.GetConnectionString("Cache"));
        }
    }
}