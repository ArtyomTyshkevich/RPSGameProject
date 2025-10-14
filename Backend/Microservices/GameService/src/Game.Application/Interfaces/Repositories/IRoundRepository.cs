using Game.Domain.Entities;

namespace Game.Application.Interfaces.Repositories
{
    public interface IRoundRepository : IRepository<Round>
    {
        Task<Round> GetByIdAsNoTrakingAsync(Guid id, CancellationToken cancellationToken = default);
    }

}