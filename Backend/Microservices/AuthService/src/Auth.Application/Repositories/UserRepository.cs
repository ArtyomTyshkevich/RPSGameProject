﻿using Auth.BLL.Interfaces;
using Auth.DAL.Data;
using Auth.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task<List<User>> Get(CancellationToken cancellationToken)
        {
            return await _authDbContext.Users
                                            .Include(user => user.UserName)
                                            .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetById(Guid userId, CancellationToken cancellationToken)
        {
            return await _authDbContext.Users
                                 .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
        }

        public async Task<List<IdentityRole<Guid>>> GetRoles(User user, CancellationToken cancellationToken)
        {
            var roles = await _authDbContext.Roles
                .Where(role => _authDbContext.UserRoles
                    .Where(contextUser => contextUser.UserId == user.Id)
                    .Select(contextUser => contextUser.RoleId)
                    .Contains(role.Id))
                .ToListAsync(cancellationToken);

            return roles;
        }

        public async Task<User> GetByMail(string mail, CancellationToken cancellationToken)
        {
            return await _authDbContext.Users
                .FirstAsync(user => user.Email == mail, cancellationToken);
        }
    }
}
