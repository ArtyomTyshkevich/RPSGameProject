using AutoMapper;
using MassTransit;
using Moq;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Repositories;
using Profile.DAL.Entities;
using Profile.BLL.Services;
using Xunit;
using Profile.BLL.Mappings;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<UserMappingProfile>());
        var mapper = new Mapper(configuration);
        _userService = new UserService(_unitOfWorkMock.Object, mapper, _publishEndpointMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfUserDTOs()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = "User1" },
            new User { Id = Guid.NewGuid(), Name = "User2" }
        };

        var userDTOs = new List<UserDTO>
        {
            new UserDTO { Id = users[0].Id, Name = "User1" },
            new UserDTO { Id = users[1].Id, Name = "User2" }
        };

        _unitOfWorkMock.Setup(uow => uow.Users.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);



        // Act
        var result = await _userService.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDTOs.Count, result.Count); 
        Assert.Equal(userDTOs[0].Id, result[0].Id);
        Assert.Equal(userDTOs[1].Id, result[1].Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUserDTO()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Name = "User1" };
        var userDTO = new UserDTO { Id = userId, Name = "User1" };

        _unitOfWorkMock.Setup(uow => uow.Users.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mapperMock.Setup(m => m.Map<UserDTO>(user))
            .Returns(userDTO);

        // Act
        var result = await _userService.GetByIdAsync(userId, CancellationToken.None);

        // Assert
        Assert.Equal(userDTO, result);
    }


    [Fact]
    public async Task UpdateRating_ShouldUpdateUserRating()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var rating = 5;
        var user = new User { Id = userId, Rating = 0 };

        _unitOfWorkMock.Setup(uow => uow.Users.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        await _userService.UpdateRating(userId, rating, CancellationToken.None);

        // Assert
        Assert.Equal(rating, user.Rating);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUsersSortedByRatingWithPagination_ShouldReturnPaginatedUsers()
    {
        // Arrange
        var page = 1;
        var pageSize = 10;
        var users = new List<User>
    {
        new User { Id = Guid.NewGuid(), Rating = 5 },
        new User { Id = Guid.NewGuid(), Rating = 4 }
    };

        var userDTOs = new List<UserDTO>
    {
        new UserDTO { Id = users[0].Id, Rating = 5 },
        new UserDTO { Id = users[1].Id, Rating = 4 }
    };

        // Мокируем метод GetUsersSortedByRatingWithPagination
        _unitOfWorkMock
            .Setup(uow => uow.Users.GetUsersSortedByRatingWithPagination(page, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _userService.GetUsersSortedByRatingWithPagination(page, pageSize, CancellationToken.None);

        // Assert
        Assert.NotNull(result); // Проверяем, что результат не null
        Assert.Equal(userDTOs.Count, result.Count); // Проверяем, что количество элементов совпадает
        Assert.Equal(userDTOs[0].Id, result[0].Id); // Проверяем, что первый элемент совпадает
        Assert.Equal(userDTOs[1].Id, result[1].Id); // Проверяем, что второй элемент совпадает
    }

    [Fact]
    public async Task GetTotalUserCountAsync_ShouldReturnTotalUserCount()
    {
        // Arrange
        var totalCount = 10;

        _unitOfWorkMock.Setup(uow => uow.Users.GetTotalUserCountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);

        // Act
        var result = await _userService.GetTotalUserCountAsync(CancellationToken.None);

        // Assert
        Assert.Equal(totalCount, result);
    }
}