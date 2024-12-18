using Game.Domain.Enums;

namespace Game.Domain.Entities
{
    public class GameRool
    {
        public int Id { get; set; }
        public PlayerMoves FirstPlayerMove { get; set; }
        public PlayerMoves SecondPlayerMove { get; set; }
        public GameResults Result { get; set; }
    }
}
