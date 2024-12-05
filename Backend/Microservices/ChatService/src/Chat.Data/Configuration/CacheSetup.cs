﻿namespace Chat.Data.Configuration
{
    public static class CacheSetup
    {
        public static void ConfigureCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(option =>
                option.Configuration = configuration.GetConnectionString("Redis"));
        }
    }
}