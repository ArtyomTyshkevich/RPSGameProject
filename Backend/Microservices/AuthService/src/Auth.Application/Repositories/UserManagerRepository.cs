using Auth.BLL.DTOs.Identity;
using Auth.BLL.Interfaces;
using Auth.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly UserManager<User> _userManager;

        public UserManagerRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> FindByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user)
        {
           return  await _userManager.AddToRoleAsync(user, RoleConsts.User);
        }

        public async Task<List<User>> UsersToListAsync(CancellationToken cancellationToken)
        {
          return await _userManager.Users.ToListAsync(cancellationToken);
        }
    }
}

