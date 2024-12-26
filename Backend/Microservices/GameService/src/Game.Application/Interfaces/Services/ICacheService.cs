using Game.Application.DTOs;

namespace Game.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task SetConnection(string connectionId, UserConnection connection);
        Task DeleteConnection(string connectionId);
        Task<UserConnection?> GetConnection(string connectionId);
    }
}