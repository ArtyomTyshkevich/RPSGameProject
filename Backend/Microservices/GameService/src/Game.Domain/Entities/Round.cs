
using Game.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Game.Domain.Entities
{
    public class Round
    {
        [Key]
        public Guid Id { get; set; }
        public PlayerMoves? FirstPlayerMove { get; set; }
        public PlayerMoves? SecondPlayerMove { get; set;}
        public GameResults? RoundResult { get; set; }
    }
}
