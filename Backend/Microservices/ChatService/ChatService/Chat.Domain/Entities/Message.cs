
namespace Chat.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
