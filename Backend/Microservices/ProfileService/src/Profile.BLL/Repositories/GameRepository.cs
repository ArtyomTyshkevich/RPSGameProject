using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Profile.BLL.Configuretion;
using Profile.BLL.Interfaces.Repositories;
using Profile.DAL.Entities.Mongo;
namespace Profile.BLL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IMongoCollection<Game> _games;

        public GameRepository(IOptions<MongoDbConfiguration> mongoDbConfiguration)
        {
            var mongoClient = new MongoClient(mongoDbConfiguration.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(mongoDbConfiguration.Value.DatabaseName);
            _games = mongoDb.GetCollection<Game>(mongoDbConfiguration.Value.GameCollectionName);
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync(CancellationToken cancellationToken = default)
        {
            return await _games.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<Game> GetGameByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _games.Find(g => g.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddGameAsync(Game game, CancellationToken cancellationToken = default)
        {
            var bsonDocument = game.ToBsonDocument();
            Console.WriteLine(bsonDocument.ToString());
            await _games.InsertOneAsync(game, cancellationToken: cancellationToken);
        }

        public async Task UpdateGameAsync(Game game, CancellationToken cancellationToken = default)
        {
            await _games.ReplaceOneAsync(g => g.Id == game.Id, game, cancellationToken: cancellationToken);
        }

        public async Task DeleteGameAsync(string id, CancellationToken cancellationToken = default)
        {
            await _games.DeleteOneAsync(g => g.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Game>> GetAllUserGamesAsync(string userId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Game>.Filter.Or(
                Builders<Game>.Filter.Eq(g => g.FirstPlayerId, userId),
                Builders<Game>.Filter.Eq(g => g.SecondPlayerId, userId)
            );

            return await _games.Find(filter).ToListAsync(cancellationToken);
        }
        public async Task<int> GetAllGamesCountAsync(string userId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Game>.Filter.Or(
                Builders<Game>.Filter.Eq(g => g.FirstPlayerId, userId),
                Builders<Game>.Filter.Eq(g => g.SecondPlayerId, userId)
            );

            var count = await _games.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            return (int)count;
        }
    }
}
    