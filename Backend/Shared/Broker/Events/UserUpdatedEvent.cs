namespace RPSGame.Broker.Events
{
    public class UserUpdatedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int Rating { get; set; }
        public string Mail { get; set; } = "";
    }
}
