using AutoMapper;
using Broker.Events;
using Game.Application.Interfaces.Buses;
using Game.Application.Interfaces.Repositories;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Data.Services;
using Game.Domain.Entities;
using Game.Domain.Enums;
using Moq;
using Xunit;

namespace Game.Tests.Integretions
{
    public class RoundServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IBrokerBus> _mockBus;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RoundService _roundService;

        public RoundServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockBus = new Mock<IBrokerBus>();
            _mockMapper = new Mock<IMapper>();
            _roundService = new RoundService(_mockUnitOfWork.Object, _mockBus.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task ProcessRound_ShouldChangeRating_WhenPlayerMadeMove()
        {
            var roomId = Guid.NewGuid();
            var firstPlayerId = Guid.NewGuid();
            var secondPlayerId = Guid.NewGuid();
            var room = new Room
            {
                Id = roomId,
                FirstPlayer = new User { Id = firstPlayerId, Rating = 1000 },
                SecondPlayer = new User { Id = secondPlayerId, Rating = 1000 },
                Rounds = new List<Round>
                {
                    new Round { FirstPlayerMove = null, SecondPlayerMove = PlayerMoves.Rock }
                },
                RoundNum = 0
            };

            var firstPlayer = new User { Id = firstPlayerId, Rating = 1000 };
            var secondPlayer = new User { Id = secondPlayerId, Rating = 1000 };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(firstPlayerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(firstPlayer);
            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(secondPlayerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(secondPlayer);

            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsNoTrakingAsync(roomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            _mockUnitOfWork.Setup(u => u.Rooms.Attach(room));

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _mockUnitOfWork.Setup(u => u.Users.ChangeRating(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Callback<Guid, int, CancellationToken>((userId, points, token) =>
                {
                    if (userId == firstPlayerId)
                    {
                        firstPlayer.Rating += points;
                        if (firstPlayer.Rating < 0) firstPlayer.Rating = 0;
                    }
                });

            _mockUnitOfWork.Setup(u => u.Rools.GetResultAsync(It.IsAny<PlayerMoves>(), It.IsAny<PlayerMoves>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GameResults.FirstPlayerWon);

            var cancellationToken = CancellationToken.None;

            var result = await _roundService.ProcessRound(room, firstPlayerId, PlayerMoves.Paper, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(PlayerMoves.Paper, result.FirstPlayerMoves);
            Assert.Equal(PlayerMoves.Rock, result.SecondPlayerMoves);
            Assert.Equal(0, result.CurrentRaundNum);
            Assert.Equal(firstPlayerId, result.CurrentRoundWinnerID);
            Assert.Equal(firstPlayerId, result.GameWinnerId);
            Assert.Equal(1025, firstPlayer.Rating);

            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ProcessRound_ShouldReturnNull_WhenOnlySecondPlayerMadeMove()
        {
            var roomId = Guid.NewGuid();
            var firstPlayerId = Guid.NewGuid();
            var secondPlayerId = Guid.NewGuid();
            var room = new Room
            {
                Id = roomId,
                FirstPlayer = new User { Id = firstPlayerId },
                SecondPlayer = new User { Id = secondPlayerId },
                Rounds = new List<Round>
                {
                    new Round { FirstPlayerMove = null, SecondPlayerMove = null }
                },
                RoundNum = 0
            };

            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsNoTrakingAsync(roomId, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(room);
            _mockUnitOfWork.Setup(u => u.Rooms.Attach(room));
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _roundService.ProcessRound(room, secondPlayerId, PlayerMoves.Rock, CancellationToken.None);

            Assert.Null(result);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DisconetedAsync_ShouldEndGame_WhenPlayerDisconnects()
        {
            var roomId = Guid.NewGuid();
            var firstPlayerId = Guid.NewGuid();
            var secondPlayerId = Guid.NewGuid();
            var room = new Room
            {
                Id = roomId,
                FirstPlayer = new User { Id = firstPlayerId, Rating = 1000 },
                SecondPlayer = new User { Id = secondPlayerId, Rating = 1000 },
                Rounds = new List<Round>
                {
                    new Round { FirstPlayerMove = null, SecondPlayerMove = null }
                },
                RoundNum = 0,
                GameResult = null
            };

            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(repo => repo.GetByIdAsync(firstPlayerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room.FirstPlayer);

            mockUserRepo.Setup(repo => repo.GetByIdAsync(secondPlayerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room.SecondPlayer);

            mockUserRepo.Setup(repo => repo.ChangeRating(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Callback<Guid, int, CancellationToken>((userId, points, token) =>
                {
                    var player = userId == firstPlayerId ? room.FirstPlayer : room.SecondPlayer;
                    if (player != null)
                    {
                        player.Rating += points;
                        if (player.Rating < 0)
                        {
                            player.Rating = 0;
                        }
                    }
                });

            _mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepo.Object);
            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsNoTrakingAsync(roomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            _mockUnitOfWork.Setup(u => u.Rooms.Attach(room));

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _mockUnitOfWork.Setup(u => u.Rools.GetResultAsync(It.IsAny<PlayerMoves>(), It.IsAny<PlayerMoves>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GameResults.FirstPlayerWon);

            _mockBus.Setup(b => b.Publish(It.IsAny<GameResultsProcessedEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var cancellationToken = CancellationToken.None;

            var result = await _roundService.DisconetedAsync(room, firstPlayerId, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(secondPlayerId, result.GameWinnerId);
            Assert.Equal(1025, room.SecondPlayer.Rating);

            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            _mockBus.Verify(b => b.Publish(It.IsAny<GameResultsProcessedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
