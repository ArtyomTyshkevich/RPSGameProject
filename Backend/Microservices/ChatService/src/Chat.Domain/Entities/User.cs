
namespace Chat.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string NickName { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
