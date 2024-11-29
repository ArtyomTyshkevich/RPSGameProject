using MediatR;
using Microsoft.AspNetCore.Identity;
using Auth.DAL.Entities;

namespace Auth.BLL.UseCases.Commands
{
    public class CreateTokenCommand : IRequest<string>
    {
        public User User { get; set; }
        public List<IdentityRole<long>> Roles { get; set; }
    }
}