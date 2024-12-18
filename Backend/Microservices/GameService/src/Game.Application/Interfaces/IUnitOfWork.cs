using Game.Application.Interfaces;

namespace Game.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IRoomRepository Rooms { get; }
        IRoundRepository Rounds { get; }
        IUserRepository Users { get; }

        void Dispose();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}