namespace Profile.BLL.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        void Dispose();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}