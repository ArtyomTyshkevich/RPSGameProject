
namespace Chat.Application.DTOs
{
    public class UserConnection
    {
        public UserDTO UserDTO {  get; set; } = new UserDTO();
        public string ChatRoom { get; set; } = "";
    }
}
