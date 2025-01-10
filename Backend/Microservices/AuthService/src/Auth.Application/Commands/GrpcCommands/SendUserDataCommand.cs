using MediatR;

namespace Auth.BLL.Commands.GrpcCommands
{
    public class SendUserDataCommand : IRequest
    {
        public string Email { get; set; }
    }
}