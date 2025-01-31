
namespace Profile.BLL.DTOs
{
    public class GameDTO
    {
        public string Id { get; set; } = string.Empty;
        public string? FirstPlayerId { get; set; }
        public string? SecondPlayerId { get; set; }
        public List<RoundDTO> Rounds { get; set; } = new List<RoundDTO>();
        public string? GameResult { get; set; }
    }
}
