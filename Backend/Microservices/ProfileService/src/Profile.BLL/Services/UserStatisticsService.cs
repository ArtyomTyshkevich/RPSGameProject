using Profile.BLL.Interfaces.Repositories;
using Profile.BLL.Interfaces.Services;
using Profile.BLL.Enums;
using Profile.BLL.DTOs;
using AutoMapper;
using Profile.DAL.Entities.Mongo;

namespace Profile.BLL.Services
{
    public class UserStatisticsService : IUserStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserStatisticsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Dictionary<PlayerMoves, int>> GetMoveStatisticsAsync(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken)
        {
            var moveStats = Enum.GetValues(typeof(PlayerMoves))
                .Cast<PlayerMoves>()
                .ToDictionary(move => move, move => 0);

            var moves = games
                .Where(game => game.FirstPlayerId == playerId.ToString() || game.SecondPlayerId == playerId.ToString())
                .SelectMany(game => game.Rounds, (game, round) =>
                {
                    if (round.FirstPlayerMove != null && game.FirstPlayerId == playerId.ToString())
                    {
                        return round.FirstPlayerMove;
                    }
                    else if (round.SecondPlayerMove != null && game.SecondPlayerId == playerId.ToString())
                    {
                        return round.SecondPlayerMove;
                    }
                    return null;
                })
                .Where(move => move != null);

            foreach (var move in moves)
            {
                var playerMove = Enum.Parse<PlayerMoves>(move);
                moveStats[playerMove]++;
            }

            return moveStats;
        }





        public async Task<int> GetWinsCountAsync(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken)
        {
            var winsCount = games
                .Where(game =>
                    (game.FirstPlayerId == playerId.ToString() && game.GameResult == GameResults.FirstPlayerWon.ToString()) ||
                    (game.SecondPlayerId == playerId.ToString() && game.GameResult == GameResults.SecondPlayerWon.ToString())
                )
                .Count();

            return winsCount;
        }

        public async Task<int> GetLossesCountAsync(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken)
        {
            var lossesCount = games
                .Where(game =>
                    (game.FirstPlayerId == playerId.ToString() && game.GameResult == GameResults.SecondPlayerWon.ToString()) ||
                    (game.SecondPlayerId == playerId.ToString() && game.GameResult == GameResults.FirstPlayerWon.ToString())
                )
                .Count();

            return lossesCount;
        }


        public async Task<double> GetWinRateAsync(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken)
        {

            var winCount = await GetWinsCountAsync(games, playerId, cancellationToken);
            var totalGames = games
                .Where(game =>
                    game.FirstPlayerId == playerId.ToString() || game.SecondPlayerId == playerId.ToString()
                )
                .Count();
            var winRate = totalGames > 0
                ? Math.Round((double)winCount / (double)totalGames * 100, 1)
                : 0;
            return winRate;
        }

        public async Task<IEnumerable<GameDTO>> GetAllUserGames(Guid playerId, CancellationToken cancellationToken)
        {
            var games = await _unitOfWork.Games.GetAllUserGamesAsync(playerId.ToString(), cancellationToken);
            return _mapper.ProjectTo<GameDTO>(games.AsQueryable()).ToList();
        }

        public async Task<UserWithStatistics> CreateUserWithStatistics(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(playerId, cancellationToken);
            var winrate = await GetWinRateAsync(games, playerId, cancellationToken);
            var moveStatistics = await GetMoveStatisticsAsync(games, playerId, cancellationToken);
            var winsCount = await GetWinsCountAsync(games, playerId, cancellationToken);
            var lossesCount = await GetLossesCountAsync(games, playerId, cancellationToken);

            moveStatistics.TryGetValue(PlayerMoves.Rock, out var rockUsage);
            moveStatistics.TryGetValue(PlayerMoves.Paper, out var paperUsage);
            moveStatistics.TryGetValue(PlayerMoves.Scissors, out var scissorsUsage);

            return new UserWithStatistics
            {
                Id = playerId,
                Name = user.Name,
                Rating = user.Rating,
                Mail = user.Mail,
                Winrate = winrate,
                RocksUsage = rockUsage,
                PaperUsage = paperUsage,
                ScissorsUsage = scissorsUsage,
                Wins = winsCount,
                Losses = lossesCount
            };
        }
    }
}
