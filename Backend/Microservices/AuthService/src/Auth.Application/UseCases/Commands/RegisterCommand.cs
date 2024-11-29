using Auth.BLL.DTOs.Identity;
using MediatR;

namespace Auth.BLL.UseCases.Commands
{
    public class RegisterCommand : IRequest<AuthResponse>
    {
        public RegisterRequest RegisterRequest { get; set; }
    }
}        //public async Task<AuthResponse> Register(RegisterRequest request, CancellationToken cancellationToken)