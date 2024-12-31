
using Game.Domain.Enums;

namespace Game.Application.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public UserStatuses Status { get; set; }
        public int Rating { get; set; }
    }
}
