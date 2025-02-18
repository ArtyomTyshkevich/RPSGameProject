using Moq;
using Profile.BLL.Interfaces.Repositories;
using Profile.BLL.Interfaces.Services;
using Profile.BLL.DTOs;
using Profile.BLL.Enums;
using Xunit;
using Profile.BLL.Services;
using AutoMapper;

namespace Profile.BLL.Tests
{
    public class UserStatisticsServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IUserStatisticsService _userStatisticsService;

        public UserStatisticsServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userStatisticsService = new UserStatisticsService(_unitOfWorkMock.Object, _mapperMock.Object); 
        }

        [Fact]
        public async Task GetMoveStatisticsAsync_ShouldReturnCorrectStatistics()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var games = new List<GameDTO>
            {
                new GameDTO
                {
                    FirstPlayerId = playerId.ToString(),
                    SecondPlayerId = Guid.NewGuid().ToString(),
                    Rounds = new List<RoundDTO>
                    {
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Paper" },
                        new RoundDTO { FirstPlayerMove = "Scissors", SecondPlayerMove = "Rock" }
                    }
                }
            };

            // Act
            var result = await _userStatisticsService.GetMoveStatisticsAsync(games, playerId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result[PlayerMoves.Rock]);
            Assert.Equal(0, result[PlayerMoves.Paper]);
            Assert.Equal(1, result[PlayerMoves.Scissors]);
        }

        [Fact]
        public async Task GetWinsCountAsync_ShouldReturnCorrectWinsCount()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var games = new List<GameDTO>
            {
                new GameDTO { FirstPlayerId = playerId.ToString(), GameResult = GameResults.SecondPlayerWon.ToString() },
                new GameDTO { FirstPlayerId = playerId.ToString(), GameResult = GameResults.FirstPlayerWon.ToString() },
                new GameDTO { SecondPlayerId = playerId.ToString(), GameResult = GameResults.SecondPlayerWon.ToString() },
                new GameDTO { SecondPlayerId = playerId.ToString(), GameResult = GameResults.FirstPlayerWon.ToString() }
            };

            // Act
            var result = await _userStatisticsService.GetWinsCountAsync(games, playerId, CancellationToken.None);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetLossesCountAsync_ShouldReturnCorrectLossesCount()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var games = new List<GameDTO>
            {
                new GameDTO { FirstPlayerId = playerId.ToString(), GameResult = GameResults.SecondPlayerWon.ToString() },
                new GameDTO { FirstPlayerId = playerId.ToString(), GameResult = GameResults.FirstPlayerWon.ToString() },
                new GameDTO { SecondPlayerId = playerId.ToString(), GameResult = GameResults.SecondPlayerWon.ToString() },
                new GameDTO { SecondPlayerId = playerId.ToString(), GameResult = GameResults.FirstPlayerWon.ToString() }
            };

            // Act
            var result = await _userStatisticsService.GetLossesCountAsync(games, playerId, CancellationToken.None);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetWinRateAsync_ShouldReturnCorrectWinRate()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var games = new List<GameDTO>
            {
                new GameDTO { FirstPlayerId = playerId.ToString(), GameResult = GameResults.FirstPlayerWon.ToString() },
                new GameDTO { SecondPlayerId = playerId.ToString(), GameResult = GameResults.SecondPlayerWon.ToString() },
                new GameDTO { FirstPlayerId = playerId.ToString(), GameResult = GameResults.SecondPlayerWon.ToString() }
            };

            // Act
            var result = await _userStatisticsService.GetWinRateAsync(games, playerId, CancellationToken.None);

            // Assert
            Assert.Equal(66.7, result); 
        }
    }
}
