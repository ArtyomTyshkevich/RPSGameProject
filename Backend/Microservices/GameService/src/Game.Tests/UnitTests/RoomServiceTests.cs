using AutoMapper;
using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Entities;
using Game.Domain.Enums;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Game.Data.Services.Tests
{
    public class RoomServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RoomService _roomService;

        public RoomServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _roomService = new RoomService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateRoomWithRoundsAsync_ShouldCreateRoomAndRounds()
        {
            var roomDTO = new RoomDTO
            {
                RoomType = RoomTypes.Default,
                RoomStatus = RoomStatuses.WaitingPlayers
            };
            var room = new Room
            {
                Tipe = RoomTypes.Default,
                Status = RoomStatuses.WaitingPlayers,
                Rounds = new List<Round>()
            };
            var numRounds = 3;

            _mockMapper.Setup(m => m.Map<Room>(roomDTO)).Returns(room);
            _mockUnitOfWork.Setup(u => u.Rooms.CreateAsync(room, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _roomService.CreateRoomWithRoundsAsync(roomDTO, numRounds, CancellationToken.None);

            _mockUnitOfWork.Verify(u => u.Rooms.CreateAsync(room, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            room.Rounds.Should().HaveCount(numRounds);
        }

        [Fact]
        public async Task AddUserToRoomAsync_ShouldAddUserToRoom_WhenRoomIsAvailable()
        {
            var user = new User { Id = Guid.NewGuid(), Name = "TestUser" };
            var room = new Room
            {
                Id = Guid.NewGuid(),
                Tipe = RoomTypes.Default,
                Status = RoomStatuses.WaitingPlayers,
                FirstPlayer = null,
                SecondPlayer = null
            };

            _mockUnitOfWork.Setup(u => u.Rooms.GetAvailableRoomAsync(RoomTypes.Default, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _roomService.AddUserToRoomAsync(user, CancellationToken.None);

            result.Should().Be(room.Id);
            room.FirstPlayer.Should().Be(user);
            room.Status.Should().Be(RoomStatuses.WaitingPlayers);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddUserToRoomAsync_ShouldReturnNull_WhenNoRoomIsAvailable()
        {
            var user = new User { Id = Guid.NewGuid(), Name = "TestUser" };

            _mockUnitOfWork.Setup(u => u.Rooms.GetAvailableRoomAsync(RoomTypes.Default, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Room)null);

            var result = await _roomService.AddUserToRoomAsync(user, CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetRoomByIdAsync_ShouldReturnRoomDTO()
        {
            var roomId = Guid.NewGuid();
            var room = new Room
            {
                Id = roomId,
                Tipe = RoomTypes.Default,
                Status = RoomStatuses.WaitingPlayers
            };
            var roomDTO = new RoomDTO
            {
                Id = roomId,
                RoomType = RoomTypes.Default,
                RoomStatus = RoomStatuses.WaitingPlayers
            };

            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(roomId, It.IsAny<CancellationToken>())).ReturnsAsync(room);
            _mockMapper.Setup(m => m.Map<RoomDTO>(room)).Returns(roomDTO);

            var result = await _roomService.GetRoomByIdAsync(roomId, CancellationToken.None);

            result.Should().BeEquivalentTo(roomDTO);
        }

        [Fact]
        public async Task DeleteRoomByIdAsync_ShouldDeleteRoom()
        {
            var roomId = Guid.NewGuid();

            _mockUnitOfWork.Setup(u => u.Rooms.DeleteAsync(roomId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _roomService.DeleteRoomByIdAsync(roomId, CancellationToken.None);

            _mockUnitOfWork.Verify(u => u.Rooms.DeleteAsync(roomId, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteInactiveRoomsAsync_ShouldDeleteInactiveRooms()
        {
            _mockUnitOfWork.Setup(u => u.Rooms.DeleteInactiveRoomsAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _roomService.DeleteInactiveRoomsAsync(CancellationToken.None);

            _mockUnitOfWork.Verify(u => u.Rooms.DeleteInactiveRoomsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRoomStatusAsync_ShouldUpdateRoomStatus()
        {
            var roomId = Guid.NewGuid();
            var newStatus = RoomStatuses.InGame;

            _mockUnitOfWork.Setup(u => u.Rooms.UpdateRoomStatusAsync(roomId, newStatus, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _roomService.UpdateRoomStatusAsync(roomId, newStatus, CancellationToken.None);

            _mockUnitOfWork.Verify(u => u.Rooms.UpdateRoomStatusAsync(roomId, newStatus, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
