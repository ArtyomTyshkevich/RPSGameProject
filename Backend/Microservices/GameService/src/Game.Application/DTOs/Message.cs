
using Game.Domain.Enums;

namespace Game.Application.DTOs
{
    public class Message
    {
        public PlayerMoves? FirstPlayerMoves { get; set; }
        public PlayerMoves? SecondPlayerMoves { get; set; }
        public int CurrentRaundNum { get; set; }
        public Guid? CurrentRoundWinnerID { get; set; }
        public Guid? GameWinnerId { get; set; }
    }
}
