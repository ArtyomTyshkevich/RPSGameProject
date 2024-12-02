using MediatR;
using Auth.DAL.Entities;
using Auth.BLL.Exceptions;
using Auth.BLL.DTOs.Identity;
using Auth.BLL.Commands;
using Auth.BLL.Queries;
using Library.Application.Interfaces;

namespace Auth.BLL.Handlers.CommandHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = CreateRegisterUser(request.RegisterRequest);
            var result = await _unitOfWork.UserManagers.CreateUserAsync(user, request.RegisterRequest.Password);
            if (!result.Succeeded) throw new UserCreationFailedException();
            await _unitOfWork.UserManagers.AddToRoleAsync(user);

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