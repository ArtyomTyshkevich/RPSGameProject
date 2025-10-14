
using Game.Domain.Enums;

namespace Game.Application.DTOs
{
    public class RoomCreatedEvent
    {
        public Guid User1Id { get; set; }
        public Guid User2Id { get; set; }
        public string RoomId { get; set; }
    }
}
