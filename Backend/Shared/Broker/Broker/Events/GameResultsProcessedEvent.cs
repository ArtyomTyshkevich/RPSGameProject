namespace Broker.Events
{
    public class GameResultsProcessedEvent
    {
        public int FirstPlayerRating { get; set; }
        public int SecondPlayerRating { get; set; }
        public GameResultDto Game { get; set; } = null!;
    }

    public class GameResultDto
    {
        public string Id { get; set; } = null!;
        public string? FirstPlayerId { get; set; }
        public string? SecondPlayerId { get; set; }
        public List<RoundResultDto> Rounds { get; set; } = new();
        public string? GameResult { get; set; }
    }

    public class RoundResultDto
    {
        public int RoundNumber { get; set; }
        public string? FirstPlayerMove { get; set; }
        public string? SecondPlayerMove { get; set; }
        public string? RoundResult { get; set; }
    }
}
