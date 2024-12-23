using Game.Application.DTOs;

namespace Game.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task CachingConnection(string connectionId, UserConnection connection);
        Task DeleteConnectionFromCache(string connectionId);
        Task<UserConnection?> GetConnectionFromCache(string connectionId);
    }
}