using Auth.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.BLL.Interfaces
{
    public interface IUserManagerRepository
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> DeleteUserAsync(User user);
        Task<User> FindByNameAsync(string name);
        Task<User> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> AddToRoleAsync(User user);
        Task<List<User>> UsersToListAsync(CancellationToken cancellationToken);
    }
}