
using Game.Domain.Enums;

namespace Game.Application.DTOs
{
    public class RoomDTO
    {
        public Guid Id { get; set; }
        public RoomTypes RoomType { get; set; }
        public RoomStatuses RoomStatus { get; set; }
    }
}
