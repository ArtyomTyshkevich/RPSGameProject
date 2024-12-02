using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Auth.DAL.Entities;
using Library.Application.Interfaces;
using Auth.BLL.Exceptions;
using Auth.BLL.Extensions;
using Auth.BLL.DTOs.Identity;
using Auth.BLL.Queries;
using Auth.BLL.Commands;

namespace Auth.BLL.Handlers.QuerieHandlers
{
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, AuthResponse>
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticateQueryHandler(IMediator mediator, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<AuthResponse> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            var user = await ValidateUserCredentials(request.AuthRequest, cancellationToken);
            var roles = await _unitOfWork.Users.GetRoles(user, cancellationToken); ;
            var createTokenCommand = new CreateTokenCommand { User = user, Roles = roles };
            var accessToken = await _mediator.Send(createTokenCommand, cancellationToken);
            UpdateUserTokenAndExpiry(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthResponse
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = accessToken,
                RefreshToken = user.RefreshToken!
            };
        }

        private async Task<User> ValidateUserCredentials(AuthRequest request, CancellationToken cancellationToken)
        {
            var managedUser = await _unitOfWork.UserManagers.FindByEmailAsync(request.Email);
            if (managedUser == null || !await _unitOfWork.UserManagers.CheckPasswordAsync(managedUser, request.Password))
            {
                throw new BadCredentialsException();
            }

            var user = await _unitOfWork.Users.GetByMail(request.Email, cancellationToken);
            if (user is null) throw new UserNotFoundException(request.Email);

            return user;
        }

        private void UpdateUserTokenAndExpiry(User user)
        {
            user.RefreshToken = _configuration.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());
        }

    }
}