using Chat.Data.Context;
using Game.Application.Interfaces.Repositories;
using Game.Domain.Entities;
using Game.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Game.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly GameDbContext _gameDbContext;

        public RoomRepository(GameDbContext context)
        {
            _gameDbContext = context;
        }
        public async Task<Room> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Rooms
                .Include(room => room.FirstPlayer)
                .Include(room => room.SecondPlayer)
                .Include(room => room.Rounds)
                .FirstAsync(room => room.Id == id, cancellationToken);
        }

        public async Task<List<Room>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Set<Room>()
                .Include(room => room.FirstPlayer)
                .Include(room => room.SecondPlayer)
                .Include(room => room.Rounds)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalActiveRoomsCountAsync(CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Rooms
                .Where(room => room.Status != RoomStatuses.Inactive)
                .CountAsync(cancellationToken);
        }


        public async Task<Room?> GetAvailableRoomAsync(RoomTypes roomType, CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Rooms
                .AsNoTracking()
                .Include(room => room.FirstPlayer)
                .Include(room => room.SecondPlayer)
                .Include(room => room.Rounds)
                .FirstOrDefaultAsync(room => room.Tipe == roomType && room.Status == RoomStatuses.WaitingPlayers, cancellationToken);
        }


        public async Task CreateAsync(Room room, CancellationToken cancellationToken = default)
        {
            await _gameDbContext.Rooms.AddAsync(room, cancellationToken);
        }

        public async Task UpdateAsync(Room room, CancellationToken cancellationToken = default)
        {
            _gameDbContext.Rooms.Update(room);
        }

        public async Task UpdateRoomStatusAsync(Guid roomId, RoomStatuses newStatus, CancellationToken cancellationToken = default)
        {
            var room = await _gameDbContext.Rooms.FindAsync(roomId, cancellationToken);
            if (room != null)
            {
                room.Status = newStatus;
                await _gameDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var room = await GetByIdAsync(id, cancellationToken);
            if (room != null)
            {
                _gameDbContext.Rooms.Remove(room);
            }
        }

        public async Task DeleteInactiveRoomsAsync(CancellationToken cancellationToken = default)
        {
            var inactiveRooms = await _gameDbContext.Rooms
                .Where(room => room.Status == RoomStatuses.Inactive)
                .ToListAsync(cancellationToken);

            _gameDbContext.Rooms.RemoveRange(inactiveRooms);
        }
        public void Attach(Room room)
        {
            _gameDbContext.Attach(room);
        }
    }
}
