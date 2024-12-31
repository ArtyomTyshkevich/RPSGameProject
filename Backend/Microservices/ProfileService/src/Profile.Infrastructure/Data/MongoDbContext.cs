using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Profile.DAL.Entities.Mongo;
using Profile.DAL.Providers;

namespace Profile.DAL.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<Game> Resumes => _database.GetCollection<Game>("game");

        static MongoDbContext()
        {
            BsonSerializer.RegisterSerializationProvider(new GuidSerializationProvider());

            var conventions = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreIfNullConvention(true),
            };

            ConventionRegistry.Register("DefaultConvetions", conventions, type => true);
        }
        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
    }
}