namespace Profile.BLL.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IGameRepository Games { get; }

        void Dispose();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}