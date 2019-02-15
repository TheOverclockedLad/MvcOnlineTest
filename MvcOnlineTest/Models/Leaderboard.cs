namespace MvcOnlineTest.Models
{
    public class Leaderboard
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int LeaderId { get; set; }

        public string LeaderName { get; set; }

        public int LeaderScore { get; set; }
    }
}