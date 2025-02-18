using Broker.Events;
using MassTransit;
using Moq;
using Profile.BLL.Interfaces.Services;
using Profile.BLL.Consumers;
using Xunit;

namespace Profile.Tests.IntegrationTests
{
    public class SaveGameResultConsumerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly SaveGameResultConsumer _consumer;

        public SaveGameResultConsumerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _gameServiceMock = new Mock<IGameService>();
            _consumer = new SaveGameResultConsumer(_userServiceMock.Object, _gameServiceMock.Object);
        }

        [Fact]
        public async Task Consume_ShouldCallUpdateRatingAndAddGameAsync()
        {
            // Arrange
            var firstPlayerId = Guid.NewGuid().ToString();
            var secondPlayerId = Guid.NewGuid().ToString();
            var firstPlayerRating = 100;
            var secondPlayerRating = 90;

            var gameResultsProcessedEvent = new GameResultsProcessedEvent
            {
                Game = new GameResultDto
                {
                    FirstPlayerId = firstPlayerId,
                    SecondPlayerId = secondPlayerId
                },
                FirstPlayerRating = firstPlayerRating,
                SecondPlayerRating = secondPlayerRating
            };

            var contextMock = new Mock<ConsumeContext<GameResultsProcessedEvent>>();
            contextMock.Setup(c => c.Message).Returns(gameResultsProcessedEvent);
            contextMock.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            _userServiceMock.Verify(
                s => s.UpdateRating(Guid.Parse(firstPlayerId), firstPlayerRating, CancellationToken.None),
                Times.Once);

            _userServiceMock.Verify(
                s => s.UpdateRating(Guid.Parse(secondPlayerId), secondPlayerRating, CancellationToken.None),
                Times.Once);

            _gameServiceMock.Verify(
                s => s.AddGameAsync(It.IsAny<GameResultDto>(), CancellationToken.None),
                Times.Once);
        }

        [Fact]
        public async Task Consume_ShouldHandleNullPlayerIdsGracefully()
        {
            // Arrange
            var firstPlayerId = Guid.NewGuid().ToString();
            var secondPlayerId = Guid.NewGuid().ToString();
            var gameResultsProcessedEvent = new GameResultsProcessedEvent
            {
                Game = new GameResultDto
                {
                    FirstPlayerId = firstPlayerId,
                    SecondPlayerId = secondPlayerId
                },
                FirstPlayerRating = 100,
                SecondPlayerRating = 90
            };

            var contextMock = new Mock<ConsumeContext<GameResultsProcessedEvent>>();
            contextMock.Setup(c => c.Message).Returns(gameResultsProcessedEvent);
            contextMock.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

            _userServiceMock.Setup(s => s.UpdateRating(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            _userServiceMock.Verify(
                s => s.UpdateRating(Guid.Parse(firstPlayerId), gameResultsProcessedEvent.FirstPlayerRating, It.IsAny<CancellationToken>()),
                Times.Once);

            _userServiceMock.Verify(
                s => s.UpdateRating(Guid.Parse(secondPlayerId), gameResultsProcessedEvent.SecondPlayerRating, It.IsAny<CancellationToken>()),
                Times.Once);

            _gameServiceMock.Verify(
                s => s.AddGameAsync(It.IsAny<GameResultDto>(), CancellationToken.None),
                Times.Once);
        }


    }
}
