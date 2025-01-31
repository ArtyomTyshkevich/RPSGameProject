using Profile.BLL.Interfaces.Repositories;
using Profile.BLL.Interfaces.Services;
using Profile.DAL.Entities.Mongo;
using Profile.BLL.Enums;

namespace Profile.BLL.Services
{
    public class UserStatisticsService : IUserStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserStatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Dictionary<PlayerMoves, int>> GetMoveStatisticsAsync(Guid playerId, CancellationToken cancellationToken)
        {
            var games = await _unitOfWork.Games.GetAllGamesAsync(cancellationToken);

            var moveStats = games
                .SelectMany(game => game.Rounds)
                .SelectMany(round => new[] { round.FirstPlayerMove, round.SecondPlayerMove })
                .Where(move => move != null)
                .GroupBy(move => move)
                .ToDictionary(
                    group => Enum.Parse<PlayerMoves>(group.Key!),
                    group => group.Count()
                );

            return moveStats;
        }


        public async Task<int> GetWinsCountAsync(Guid playerId, CancellationToken cancellationToken)
        {
            var games = await _unitOfWork.Games.GetAllGamesAsync(cancellationToken);

            var winsCount = games
                .Where(game =>
                    (game.FirstPlayerId == playerId.ToString() && game.GameResult == GameResults.FirstPlayerWon.ToString()) ||
                    (game.SecondPlayerId == playerId.ToString() && game.GameResult == GameResults.SecondPlayerWon.ToString())
                )
                .Count();

            return winsCount;
        }

        public async Task<int> GetLossesCountAsync(Guid playerId, CancellationToken cancellationToken)
        {
            var games = await _unitOfWork.Games.GetAllGamesAsync(cancellationToken);

            var lossesCount = games
                .Where(game =>
                    (game.FirstPlayerId == playerId.ToString() && game.GameResult == GameResults.SecondPlayerWon.ToString()) ||
                    (game.SecondPlayerId == playerId.ToString() && game.GameResult == GameResults.FirstPlayerWon.ToString())
                )
                .Count();

            return lossesCount;
        }


        public async Task<double> GetWinRateAsync(Guid playerId, CancellationToken cancellationToken)
        {
            var games = await _unitOfWork.Games.GetAllGamesAsync(cancellationToken);

            var winCount = games
                .Where(game =>
                    (game.FirstPlayerId == playerId.ToString() && game.GameResult == GameResults.FirstPlayerWon.ToString()) ||
                    (game.SecondPlayerId == playerId.ToString() && game.GameResult == GameResults.SecondPlayerWon.ToString())
                )
                .Count();
            var totalGames = games
                .Where(game =>
                    game.FirstPlayerId == playerId.ToString() || game.SecondPlayerId == playerId.ToString()
                )
                .Count();

            return totalGames > 0 ? Math.Round((double)winCount / totalGames, 2) : 0.0;
        }

    }
}
