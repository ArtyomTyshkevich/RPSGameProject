
using Game.Domain.Enums;

namespace Game.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public int RoomNum { get; set; }
        public User? FirstPlayer { get; set; }
        public User? SecondPlayer { get; set; }
        public RoomTypes RoomTipe { get; set; }
        public RoomStatuses RoomStatus { get; set; }
        public int numRounds {  get; set; } 
        public List<Round> Rounds { get; set; }
        public List<GameResults> GameResult { get; set; }
    }
}
