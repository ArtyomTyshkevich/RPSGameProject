
using Game.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Game.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public UserStatuses Status {  get; set; }
        public int Rating {  get; set; }

    }
}
