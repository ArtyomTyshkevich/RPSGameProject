using Auth.BLL.Commands;
using Auth.DAL.Entities;
using Library.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.BLL.Handlers.CommandHandlers
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeTokenCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserManagers.FindByNameAsync(request.Username);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            user.RefreshToken = null;
            var result = await _unitOfWork.UserManagers.UpdateUserAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to update user token");
            }

            return Unit.Value;
        }
    }

    public class RevokeAllTokensCommandHandler : IRequestHandler<RevokeAllTokensCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RevokeAllTokensCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RevokeAllTokensCommand request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserManagers.UsersToListAsync(cancellationToken);

            var updateTasks = users.Select(user =>
            {
                user.RefreshToken = null;
                return _unitOfWork.UserManagers.UpdateUserAsync(user);
            });

            var results = await Task.WhenAll(updateTasks);

            if (results.Any(result => !result.Succeeded))
            {
                throw new InvalidOperationException("Failed to update some user tokens");
            }

            return Unit.Value;
        }
    }
}