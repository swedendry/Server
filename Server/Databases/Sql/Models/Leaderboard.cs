using Server.Databases.Sql.Core;
using System.ComponentModel.DataAnnotations;

namespace Server.Databases.Sql.Models
{
    public class Leaderboard : IEntity
    {
        [Key]
        [MaxLength(60)]
        public string Id { get; set; }

        [Required]
        public LeaderboardOrder Order { get; set; }

        [Required]
        public LeaderboardScoreType ScoreType { get; set; }
    }

    public class LeaderboardViewModel
    {
        public string Id { get; set; }
        public LeaderboardOrder Order { get; set; }
        public LeaderboardScoreType ScoreType { get; set; }
    }

    public enum LeaderboardOrder
    {
        Ascending = 0,
        Descending = 1
    }

    public enum LeaderboardScoreType
    {
        Overwriting,
        HighScore,
        Increment,
        Decrement,
    }
}
