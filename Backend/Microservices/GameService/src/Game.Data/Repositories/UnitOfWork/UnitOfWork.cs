using Chat.Data.Context;
using Game.Application.Interfaces.Repositories;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Data.Repositories;

namespace Auth.BLL.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameDbContext _gameDbContext;

        public UnitOfWork(GameDbContext gameDbContext)
        {
            _gameDbContext = gameDbContext;
            Users = new UserRepository(_gameDbContext);
            Rounds = new RoundRepository(_gameDbContext);
            Rooms = new RoomRepository(_gameDbContext);
            Rools = new GameRoolRepository(_gameDbContext);
        }

        public IUserRepository Users { get; private set; }
        public IRoundRepository Rounds { get; private set; }
        public IRoomRepository Rooms { get; private set; }
        public IGameRoolRepository Rools { get; private set; }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _gameDbContext.SaveChangesAsync(cancellationToken);
        public void Dispose() => _gameDbContext.Dispose();
    }
}