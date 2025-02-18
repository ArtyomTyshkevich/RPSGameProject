using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Data.Consumers;
using Game.Domain.Enums;
using MassTransit;
using Moq;
using Xunit;

namespace Game.Tests.Consumers
{
    public class GameSearchConsumerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRoomService> _mockRoomService;
        private readonly Mock<ConsumeContext<UserDTO>> _mockContext;
        private readonly GameSearchConsumer _consumer;

        public GameSearchConsumerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRoomService = new Mock<IRoomService>();
            _mockContext = new Mock<ConsumeContext<UserDTO>>();
            _consumer = new GameSearchConsumer(_mockUnitOfWork.Object, _mockRoomService.Object);
        }

        [Fact]
        public async Task Consume_ShouldRespondWithRoomId_WhenUserIsInSearchAndRoomIsAvailable()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDTO = new UserDTO { Id = userId, Status = UserStatuses.InSearch };
            var roomId = Guid.NewGuid();

            // Mock the ConsumeContext
            _mockContext.Setup(c => c.Message).Returns(userDTO);
            _mockContext.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

            // Mock the UnitOfWork
            var user = new Game.Domain.Entities.User
            {
                Id = userId,
                Status = UserStatuses.InSearch
            };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdNoTrackingAsync(userId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

            // Mock the RoomService
            _mockRoomService.Setup(r => r.AddUserToRoomAsync(It.IsAny<Game.Domain.Entities.User>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(roomId);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(c => c.RespondAsync(It.Is<RoomResponse>(r => r.RoomId == roomId)), Times.Once);
        }

        [Fact]
        public async Task Consume_ShouldNotRespond_WhenUserStatusIsNotInSearch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDTO = new UserDTO { Id = userId, Status = UserStatuses.Active };

            // Mock the ConsumeContext
            _mockContext.Setup(c => c.Message).Returns(userDTO);
            _mockContext.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

            // Mock the UnitOfWork
            var user = new Game.Domain.Entities.User
            {
                Id = userId,
                Status = UserStatuses.Active
            };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdNoTrackingAsync(userId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(c => c.RespondAsync(It.IsAny<RoomResponse>()), Times.Never);
        }

        [Fact]
        public async Task Consume_ShouldRetryUntilRoomIsAvailable_WhenUserStatusIsInSearch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDTO = new UserDTO { Id = userId, Status = UserStatuses.InSearch };
            var roomId = Guid.NewGuid();

            // Mock the ConsumeContext
            _mockContext.Setup(c => c.Message).Returns(userDTO);
            _mockContext.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

            // Mock the UnitOfWork
            var user = new Game.Domain.Entities.User
            {
                Id = userId,
                Status = UserStatuses.InSearch
            };

            _mockUnitOfWork.Setup(u => u.Users.GetByIdNoTrackingAsync(userId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

            // Mock RoomService to simulate that it takes time to add the user to a room
            _mockRoomService.SetupSequence(r => r.AddUserToRoomAsync(It.IsAny<Game.Domain.Entities.User>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Guid?)null) // First attempt, no room available
                            .ReturnsAsync(roomId); // Second attempt, room available

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(c => c.RespondAsync(It.Is<RoomResponse>(r => r.RoomId == roomId)), Times.Once);
        }
    }
}
