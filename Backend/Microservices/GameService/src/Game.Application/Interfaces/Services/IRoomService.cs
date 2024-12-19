using Game.Domain.Entities;

namespace Game.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task CreateRoomWithRounds(Room room, int numRounds, CancellationToken cancellationToken);
    }
}