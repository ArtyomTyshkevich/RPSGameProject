
using Game.Domain.Enums;

namespace Game.Domain.Entities
{
    public class Round
    {
        public Guid Id { get; set; }
        public PlayerMoves? FirstPlayerMove { get; set; }
        public PlayerMoves? SecondPlayerMove { get; set;}
        public GameResults? RoundResult { get; set; }
    }
}
