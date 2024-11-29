using Auth.BLL.DTOs.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.BLL.UseCases.Queries
{
    public class AuthenticateQuery : IRequest<AuthResponse>
    {
        public AuthRequest AuthRequest { get; set; }
        public List<IdentityRole<long>> Roles { get; set; }
    }
}