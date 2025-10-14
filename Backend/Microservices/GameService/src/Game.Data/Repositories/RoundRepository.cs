using Chat.Data.Context;
using Game.Application.Interfaces.Repositories;
using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.Data.Repositories
{
    public class RoundRepository : IRoundRepository
    {
        private readonly GameDbContext _context;

        public RoundRepository(GameDbContext context)
        {
            _context = context;
        }

        public async Task<Round> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Rounds
                .FirstAsync(r => r.Id == id, cancellationToken);
        }
        public async Task<Round> GetByIdAsNoTrakingAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Rounds
                .AsNoTracking()
                .FirstAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<Round>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Rounds.ToListAsync(cancellationToken);
        }

        public async Task CreateAsync(Round round, CancellationToken cancellationToken = default)
        {
            await _context.Rounds.AddAsync(round, cancellationToken);
        }

        public async Task UpdateAsync(Round round, CancellationToken cancellationToken = default)
        {
            _context.Rounds.Update(round);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var round = await GetByIdAsync(id, cancellationToken);
            if (round != null)
            {
                _context.Rounds.Remove(round);
            }
        }
    }
}