using MongoDB.Driver;
using Profile.DAL.Entities.Mongo;

namespace Profile.DAL.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Game> Games =>
            _database.GetCollection<Game>("Games");
    }
}