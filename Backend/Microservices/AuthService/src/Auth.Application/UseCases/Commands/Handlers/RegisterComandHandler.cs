using MediatR;
using Microsoft.AspNetCore.Identity;
using Auth.DAL.Entities;
using Auth.BLL.UseCases.Queries;
using Auth.BLL.Exceptions;
using Auth.BLL.DTOs.Identity;

namespace Auth.BLL.UseCases.Commands.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public RegisterCommandHandler(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = CreateRegisterUser(request.RegisterRequest);
            var result = await _userManager.CreateAsync(user, request.RegisterRequest.Password);
            if (!result.Succeeded) throw new UserCreationFailedException();
            await _userManager.AddToRoleAsync(user, RoleConsts.User);

            var authQuery = new AuthenticateQuery
            {
                AuthRequest = new AuthRequest
                {
                    Email = request.RegisterRequest.Email,
                    Password = request.RegisterRequest.Password
                }
            };
            return await _mediator.Send(authQuery, cancellationToken);
        }

        private User CreateRegisterUser(RegisterRequest request)
        {
            return new User
            {
                Nickname = request.Nickname,
                Email = request.Email,
                UserName = request.Email
            };
        }

    }
}