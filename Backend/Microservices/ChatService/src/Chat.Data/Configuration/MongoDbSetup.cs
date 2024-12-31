using Chat.Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class MongoDbSetup
{
    public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MongoDbContext>(sp =>
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"];
            return new MongoDbContext(connectionString, databaseName);
        });
    }
}