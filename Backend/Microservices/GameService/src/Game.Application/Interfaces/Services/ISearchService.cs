
namespace Game.Application.Interfaces.Services
{
    public interface ISearchService
    {
        Task StartSearchGame(Guid userId, CancellationToken cancellationToken);
        Task StopSearchGame(Guid userId, CancellationToken cancellationToken);
    }
}