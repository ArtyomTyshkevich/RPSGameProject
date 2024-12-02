﻿using Auth.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.BLL.Interfaces
{
    public interface IUserRepository
    {
        Task Create(User user, CancellationToken cancellationToken);
        Task Delete(User user, CancellationToken cancellationToken);
        Task<List<User>> Get(CancellationToken cancellationToken);
        Task<User?> GetById(Guid userId, CancellationToken cancellationToken);
        Task<List<User>> GetWithPagination(int pageNum, int pageSize, CancellationToken cancellationToken);
        Task Update(User user, CancellationToken cancellationToken);
        Task<List<IdentityRole<Guid>>> GetRoles(User user, CancellationToken cancellationToken);
        Task<User> GetByMail(string mail, CancellationToken cancellationToken);

    }
}