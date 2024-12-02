using MediatR;
using Microsoft.AspNetCore.Identity;
using Auth.DAL.Entities;

namespace Auth.BLL.Commands
{
    public class CreateTokenCommand : IRequest<string>
    {
        public User User { get; set; }
        public List<IdentityRole<Guid>> Roles { get; set; }
    }
}