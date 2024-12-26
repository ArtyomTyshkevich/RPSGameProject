using Game.Application.DTOs;
using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task CreateRoomWithRoundsAsync(RoomDTO roomDTO, int numRounds, CancellationToken cancellationToken);
        Task<bool> AddUserToRoomAsync(User user, CancellationToken cancellationToken);
        Task<List<RoomDTO>> GetAllRoomsAsync(CancellationToken cancellationToken);
        Task<RoomDTO?> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken);
        Task DeleteRoomByIdAsync(Guid id, CancellationToken cancellationToken);
        Task DeleteInactiveRoomsAsync(CancellationToken cancellationToken);
        Task UpdateRoomStatusAsync(Guid roomId, RoomStatuses newStatus, CancellationToken cancellationToken);
    }
}