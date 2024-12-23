
namespace Game.Application.Interfaces.Services
{
    public interface ISearchService
    {
        Task StartSearchGame(Guid userID, CancellationToken cancellationToken);
        Task StopSearchGame(Guid userID, CancellationToken cancellationToken);
    }
}