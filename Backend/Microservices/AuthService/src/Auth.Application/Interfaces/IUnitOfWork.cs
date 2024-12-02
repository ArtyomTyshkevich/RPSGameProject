using Auth.BLL.Interfaces;
using Library.Data.Repositories;

namespace Library.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IUserManagerRepository UserManagers { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}