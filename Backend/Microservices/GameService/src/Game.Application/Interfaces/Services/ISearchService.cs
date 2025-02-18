
namespace Game.Application.Interfaces.Services
{
    public interface ISearchService
    {
        Task<string?> StartSearchGame(Guid userID, CancellationToken cancellationToken);
        Task StopSearchGame(Guid userId, CancellationToken cancellationToken);
    }
}