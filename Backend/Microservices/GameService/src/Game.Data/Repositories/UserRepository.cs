using Chat.Data.Context;
using Game.Application.Interfaces.Repositories;
using Game.Domain.Entities;
using Game.Domain.Enums;
using Microsoft.EntityFrameworkCore;

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

        public async Task<User> GetByIdNoTrackingAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Users
                .AsNoTracking()
                .FirstAsync(user => user.Id == userId, cancellationToken);
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Users
                .ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            await _gameDbContext.Users
                .AddAsync(user, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _gameDbContext.Users
                .Update(user);
        }

        public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _gameDbContext.Users
                .FindAsync(userId, cancellationToken);

            if (user != null)
            {
                _gameDbContext.Users
                    .Remove(user);
            }
        }

        public async Task UpdateUserStatusAsync(Guid userId, UserStatuses newStatus, CancellationToken cancellationToken = default)
        {
            var user = await _gameDbContext.Users
                .FirstAsync(user => user.Id == userId, cancellationToken);

            if (user != null)
            {
                user.Status = newStatus;
            }
        }

        public async Task ChangeRating(Guid userId, int points, CancellationToken cancellationToken)
        {
            var user = await _gameDbContext.Users
                .FirstAsync(user => user.Id == userId, cancellationToken);

            if (user != null)
            {
                user.Rating += points;

                if (user.Rating < 0)
                {
                    user.Rating = 0;
                }
            }
        }
    }
}
