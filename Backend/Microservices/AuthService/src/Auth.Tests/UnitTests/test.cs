using Auth.BLL.Commands;
using Auth.BLL.Handlers.CommandHandlers;
using Auth.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class CreateTokenCommandHandlerTests
{
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly CreateTokenCommandHandler _handler;

    public CreateTokenCommandHandlerTests()
    {
        // Настроим Mock конфигурацию для работы с Jwt
        _mockConfig = new Mock<IConfiguration>();

        // Создаем реальный IConfigurationSection для Expire
        var expireSection = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "Expire", "30" } })
            .Build()
            .GetSection("Expire");

        // Мок секции Jwt
        var mockJwtSection = new Mock<IConfigurationSection>();
        mockJwtSection.Setup(s => s.GetSection("Expire")).Returns(expireSection); // Реальная секция для Get<int>()
        mockJwtSection.Setup(s => s["Secret"]).Returns("superSecretKey@345432242344324245");
        mockJwtSection.Setup(s => s["Issuer"]).Returns("https://localhost:5001");
        mockJwtSection.Setup(s => s["Audience"]).Returns("https://localhost:5001");
        mockJwtSection.Setup(s => s["TokenValidityInMinutes"]).Returns("1");

        // Настройка GetSection для Jwt
        _mockConfig.Setup(c => c.GetSection("Jwt")).Returns(mockJwtSection.Object);

        _handler = new CreateTokenCommandHandler(_mockConfig.Object);
    }

    [Fact]
    public async Task Handle_ValidUserAndRoles_ReturnsValidToken()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "test-user",
            Email = "test@example.com"
        };

        var roles = new List<IdentityRole<Guid>>
        {
            new IdentityRole<Guid> { Name = "Admin" },
            new IdentityRole<Guid> { Name = "User" }
        };

        var command = new CreateTokenCommand { User = user, Roles = roles };

        // Act
        var token = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(token);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        // Проверка значений, включая Issuer, Audience и Claims
        Assert.Equal("https://localhost:5001", jwtToken.Issuer);
        Assert.Equal("https://localhost:5001", jwtToken.Audiences.First());
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Name && c.Value == "test-user");
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");

        // Проверка срока действия токена
        var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
        Assert.NotNull(expClaim);

        var expUnix = long.Parse(expClaim);
        var expTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

        // Т.к. между временем генерации и проверкой может пройти несколько миллисекунд,
        // допускаем небольшой промежуток времени, например, 1 минута
        var expire = _mockConfig.Object.GetSection("Jwt:Expire").Get<int>(); // Используем реальный Get<int>()
        var expectedExpire = DateTime.UtcNow.AddMinutes(expire); // Используем извлечённое значение
        Assert.True(expTime > DateTime.UtcNow && expTime <= expectedExpire);
    }
}