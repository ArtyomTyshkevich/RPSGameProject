using Profile.BLL.DTOs;

namespace Profile.BLL.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(UserDTO userDTO, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateRating(Guid id, int rating, CancellationToken cancellationToken);
    }
}