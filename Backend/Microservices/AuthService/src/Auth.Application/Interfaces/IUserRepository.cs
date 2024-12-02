using Auth.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.BLL.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> Get(CancellationToken cancellationToken);
        Task<User?> GetById(Guid userId, CancellationToken cancellationToken);
        Task<List<IdentityRole<Guid>>> GetRoles(User user, CancellationToken cancellationToken);
        Task<User> GetByMail(string mail, CancellationToken cancellationToken);

    }
}