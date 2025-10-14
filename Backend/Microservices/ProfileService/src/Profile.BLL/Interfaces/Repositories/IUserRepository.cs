using Profile.DAL.Entities;

namespace Profile.BLL.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task<List<User>> GetUsersSortedByRatingWithPagination(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<int> GetTotalUserCountAsync(CancellationToken cancellationToken = default);
    }
}