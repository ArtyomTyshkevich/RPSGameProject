using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.DAL.Data;

public static class MongoDbSetup
{
    public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MongoDbContext>(sp =>
        {
            var connectionString = configuration["MongoDbSettings:ConnectionString"];
            var databaseName = configuration["MongoDbSettings:DatabaseName"];
            return new MongoDbContext(connectionString, databaseName);
        });
    }
}