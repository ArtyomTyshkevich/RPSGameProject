using Profile.BLL.DTOs;
using Profile.DAL.Entities;

namespace Profile.BLL.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(UserDTO userDTO, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateRating(Guid id, int rating, CancellationToken cancellationToken);
        Task<List<UserDTO>> GetUsersSortedByRatingWithPagination(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<int> GetTotalUserCountAsync(CancellationToken cancellationToken = default);
    }
}