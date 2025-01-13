using Chat.Application.DTOs;
using MongoDB.Driver;

namespace Chat.Data.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Message> Messages => 
            _database.GetCollection<Message>("Messages");
    }
}