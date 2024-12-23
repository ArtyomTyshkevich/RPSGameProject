
namespace Game.Application.Interfaces.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRoomRepository Rooms { get; }
        IRoundRepository Rounds { get; }
        IUserRepository Users { get; }
        IGameRuleRepository Rools { get; }

        void Dispose();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}