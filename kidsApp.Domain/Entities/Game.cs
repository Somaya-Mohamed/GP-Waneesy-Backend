using System.Collections.Generic;

namespace kidsApp.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;           
        public string GameLink { get; set; } = string.Empty;           
        public string DifficultyLevel { get; set; } = string.Empty;
        public int PointsRewarded { get; set; }

        // Navigation Property
        public ICollection<GameScore> Scores { get; set; } = new List<GameScore>();
    }
}