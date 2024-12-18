using Chat.Data.Context;
using Game.Application.Interfaces;
using Game.Domain.Entities;
using Game.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GameDbContext _gameDbContext;

        public UserRepository(GameDbContext gameDbContext)
        {
            _gameDbContext = gameDbContext;
        }

        public async Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Users
                                 .FirstAsync(user => user.Id == userId, cancellationToken);
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Users
                .ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            var entity = await _gameDbContext.Users.AddAsync(user, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            var entity = _gameDbContext.Users.Update(user);
        }

        public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _gameDbContext.Users.FindAsync(userId, cancellationToken);
            if (user != null)
            {
                _gameDbContext.Users.Remove(user);
            }
        }
    }
}
