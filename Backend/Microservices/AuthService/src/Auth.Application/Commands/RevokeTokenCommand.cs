using MediatR;

namespace Auth.BLL.Commands
{
    public class RevokeTokenCommand : IRequest<Unit>
    {
        public string Username { get; set; }
    }

    public class RevokeAllTokensCommand : IRequest<Unit>
    {
    }
}