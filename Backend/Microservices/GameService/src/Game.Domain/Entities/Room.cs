
using Game.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Game.Domain.Entities
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }
        public User? FirstPlayer { get; set; } = null;
        public User? SecondPlayer { get; set; } = null;
        public RoomTypes Tipe { get; set; }
        public RoomStatuses Status { get; set; }
        public int RoundNum {  get; set; }
        public List<Round> Rounds { get; set; } = new List<Round>();
        public GameResults? GameResult { get; set; } = null;
    }
}
