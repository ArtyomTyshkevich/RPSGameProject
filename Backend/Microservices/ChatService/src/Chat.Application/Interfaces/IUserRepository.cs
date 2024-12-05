using Chat.Domain.Entities;

namespace Chat.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task AddAsync(User user);
    }
}
