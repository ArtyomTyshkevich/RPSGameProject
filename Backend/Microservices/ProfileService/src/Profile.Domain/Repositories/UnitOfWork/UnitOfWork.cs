using Profile.BLL.Interfaces.Repositories;
using Profile.DAL.Data;

namespace Profile.BLL.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProfileDbContext _gameDbContext;

        public UnitOfWork(ProfileDbContext gameDbContext)
        {
            _gameDbContext = gameDbContext;
            Users = new UserRepository(_gameDbContext);
        }

        public IUserRepository Users { get; private set; }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _gameDbContext.SaveChangesAsync(cancellationToken);
        public void Dispose() => _gameDbContext.Dispose();
    }
}