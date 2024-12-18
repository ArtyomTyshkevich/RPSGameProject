
using Game.Domain.Enums;

namespace Game.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public UserStatuses Status {  get; set; }
        public int Rating {  get; set; }

    }
}
