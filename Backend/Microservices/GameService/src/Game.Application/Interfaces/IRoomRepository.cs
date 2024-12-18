using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Application.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<int> GetTotalActiveRoomsCountAsync(CancellationToken cancellationToken = default);
        Task<Room?> GetAvailableRoomAsync(RoomTypes roomType, CancellationToken cancellationToken = default);
        Task DeleteInactiveRoomsAsync(CancellationToken cancellationToken = default);
    }
}