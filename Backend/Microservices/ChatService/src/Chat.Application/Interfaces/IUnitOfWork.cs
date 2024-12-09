
namespace Chat.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMessageRepository Messages { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}