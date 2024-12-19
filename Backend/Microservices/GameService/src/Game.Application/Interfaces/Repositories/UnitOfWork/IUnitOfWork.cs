using Game.Application.Interfaces.Repositories;

namespace Game.Application.Interfaces.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRoomRepository Rooms { get; }
        IRoundRepository Rounds { get; }
        IUserRepository Users { get; }
        IGameRoolRepository Rools { get; }

        void Dispose();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}