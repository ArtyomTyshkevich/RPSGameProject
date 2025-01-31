using MongoDB.Driver;
using Profile.BLL.Enums;
using Profile.BLL.Interfaces.Repositories;
using Profile.BLL.Interfaces.Services;

namespace Profile.BLL.Services
{
    public class GlobalStatisticsServices : IGlobalStatisticsServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public GlobalStatisticsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string?> GetMostUsedMoveAsync(CancellationToken cancellationToken = default)
        {
            var games = await _unitOfWork.Games.GetAllGamesAsync(cancellationToken);

            return games
                .SelectMany(game => game.Rounds)
                .SelectMany(round => new[] { round.FirstPlayerMove, round.SecondPlayerMove })
                .Where(move => move != null)
                .GroupBy(move => move)
                .OrderByDescending(group => group.Count())
                .FirstOrDefault()?.Key;
        }

        public async Task<Dictionary<PlayerMoves, double>> GetMoveWinRateAsync(CancellationToken cancellationToken = default)
        {
            var games = await _unitOfWork.Games.GetAllGamesAsync(cancellationToken);

            var moveWinRates = games
                .SelectMany(game => game.Rounds)
                .SelectMany(round => new[]
                {
                new { Move = round.FirstPlayerMove, IsWin = round.RoundResult == GameResults.FirstPlayerWon.ToString() },
                new { Move = round.SecondPlayerMove, IsWin = round.RoundResult == GameResults.SecondPlayerWon.ToString() }
                })
                .Where(x => x.Move != null)
                .GroupBy(x => x.Move)
                .ToDictionary(
                    group => Enum.Parse<PlayerMoves>(group.Key!),
                    group => group.Average(x => x.IsWin ? 1.0 : 0.0)
                );

            return moveWinRates;
        }

        public async Task<Dictionary<PlayerMoves, int>> GetMoveUsageStatisticsAsync(CancellationToken cancellationToken = default)
        {
            var games = await _unitOfWork.Games.GetAllGamesAsync(cancellationToken);

            var moveUsageStats = games
                .SelectMany(game => game.Rounds)
                .SelectMany(round => new[] { round.FirstPlayerMove, round.SecondPlayerMove })
                .Where(move => move != null)
                .GroupBy(move => move)
                .ToDictionary(
                    group => Enum.Parse<PlayerMoves>(group.Key!),
                    group => group.Count()
                );

            return moveUsageStats;
        }
    }
}