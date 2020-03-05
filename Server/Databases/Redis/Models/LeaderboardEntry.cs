namespace Server.Databases.Redis.Models
{
    public class LeaderboardEntry
    {
        public string LeaderboardId { get; set; }
        public string Member { get; set; }
        public long Rank { get; set; }
        public double Score { get; set; }
    }
}
