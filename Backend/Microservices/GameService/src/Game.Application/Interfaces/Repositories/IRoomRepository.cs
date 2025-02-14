using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Application.Interfaces.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<int> GetTotalActiveRoomsCountAsync(CancellationToken cancellationToken = default);
        Task<Room?> GetAvailableRoomAsync(RoomTypes roomType, CancellationToken cancellationToken = default);
        Task DeleteInactiveRoomsAsync(CancellationToken cancellationToken = default);
        Task UpdateRoomStatusAsync(Guid roomId, RoomStatuses newStatus, CancellationToken cancellationToken = default);
        void Attach(Room room);
        Task CleanRoomsInPreparationAsync(CancellationToken cancellationToken = default);
        Task<Room> GetByIdAsNoTrakingAsync(Guid id, CancellationToken cancellationToken = default);
    }
}