using Chat.Data.Context;

public static class MongoDbSetup
{
    public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MongoDbContext>(sp =>
        {
            var connectionString = configuration["MessageStoreDatabase:ConnectionString"];
            var databaseName = configuration["MessageStoreDatabase:DatabaseName"];
            return new MongoDbContext(connectionString, databaseName);
        });
    }
}