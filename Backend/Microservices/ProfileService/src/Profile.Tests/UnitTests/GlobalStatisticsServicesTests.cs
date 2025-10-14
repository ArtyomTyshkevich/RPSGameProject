using Moq;
using Profile.BLL.Enums;
using Profile.BLL.Services;
using Profile.BLL.DTOs;
using Xunit;
using Profile.BLL.Interfaces.Repositories;

namespace Profile.Tests
{
    public class GlobalStatisticsServicesTests
    {
        private readonly GlobalStatisticsServices _globalStatisticsServices;

        public GlobalStatisticsServicesTests()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            _globalStatisticsServices = new GlobalStatisticsServices(unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetMostUsedMoveAsync_Returns_MostUsedMove()
        {
            // Arrange
            var games = new List<GameDTO>
            {
                new GameDTO
                {
                    Rounds = new List<RoundDTO>
                    {
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Scissors" },
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Paper" }
                    }
                },
                new GameDTO
                {
                    Rounds = new List<RoundDTO>
                    {
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Scissors" },
                        new RoundDTO { FirstPlayerMove = "Paper", SecondPlayerMove = "Rock" }
                    }
                }
            };

            // Act
            var result = await _globalStatisticsServices.GetMostUsedMoveAsync(games, CancellationToken.None);

            // Assert
            Assert.Equal("Rock", result);
        }

        [Fact]
        public async Task GetMoveWinRateAsync_Returns_MoveWinRates()
        {
            // Arrange
            var games = new List<GameDTO>
            {
                new GameDTO
                {
                    Rounds = new List<RoundDTO>
                    {
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Scissors", RoundResult = GameResults.FirstPlayerWon.ToString() },
                        new RoundDTO { FirstPlayerMove = "Paper", SecondPlayerMove = "Rock", RoundResult = GameResults.FirstPlayerWon.ToString() }
                    }
                },
                new GameDTO
                {
                    Rounds = new List<RoundDTO>
                    {
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Scissors", RoundResult = GameResults.FirstPlayerWon.ToString() },
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Paper", RoundResult = GameResults.SecondPlayerWon.ToString() }
                    }
                }
            };

            // Act
            var result = await _globalStatisticsServices.GetMoveWinRateAsync(games, CancellationToken.None);

            // Assert
            Assert.Contains(result, kvp => kvp.Key == PlayerMoves.Rock && kvp.Value == 50);
            Assert.Contains(result, kvp => kvp.Key == PlayerMoves.Paper && kvp.Value == 100);
        }

        [Fact]
        public async Task GetMoveUsageStatisticsAsync_Returns_MoveUsageStats()
        {
            // Arrange
            var games = new List<GameDTO>
            {
                new GameDTO
                {
                    Rounds = new List<RoundDTO>
                    {
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Scissors" },
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Paper" }
                    }
                },
                new GameDTO
                {
                    Rounds = new List<RoundDTO>
                    {
                        new RoundDTO { FirstPlayerMove = "Rock", SecondPlayerMove = "Scissors" },
                        new RoundDTO { FirstPlayerMove = "Paper", SecondPlayerMove = "Rock" }
                    }
                }
            };

            // Act
            var result = await _globalStatisticsServices.GetMoveUsageStatisticsAsync(games, CancellationToken.None);

            // Assert
            Assert.Contains(result, kvp => kvp.Key == PlayerMoves.Rock && kvp.Value == 4);
            Assert.Contains(result, kvp => kvp.Key == PlayerMoves.Paper && kvp.Value == 2);
            Assert.Contains(result, kvp => kvp.Key == PlayerMoves.Scissors && kvp.Value == 2);
        }
    }
}
