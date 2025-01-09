using Microsoft.Extensions.Options;
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
    }
}
    