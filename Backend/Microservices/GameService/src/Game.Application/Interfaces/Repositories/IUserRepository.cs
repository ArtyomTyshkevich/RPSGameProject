using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdateUserStatusAsync(Guid userId, UserStatuses newStatus, CancellationToken cancellationToken = default);
        Task ChangeReting(Guid userId, int points, CancellationToken cancellationToken);
        Task<User> GetByIdNoTrackingAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}