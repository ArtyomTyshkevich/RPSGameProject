using Chat.Domain.Entities;

namespace Chat.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<bool> DeleteAsync(Guid userId);
        Task<User> GetByIdAsync(Guid userId);
        Task<User> UpdateAsync(User user);
    }
}