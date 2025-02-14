namespace Profile.BLL.DTOs
{
    public class UserWithStatistics
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int Rating { get; set; }
        public string Mail { get; set; } = "";
        public double Winrate { get; set; }
        public int Wins {  get; set; }
        public int Losses { get; set; }
        public int RocksUsage { get; set; }
        public int PaperUsage { get; set; }
        public int ScissorsUsage { get; set; }
    }
}
