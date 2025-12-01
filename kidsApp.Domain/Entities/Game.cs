using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain
{
    public class Game
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string GameLink { get; set; }
        public string DifficultyLevel { get; set; }
        public int PointsRewarded { get; set; }

        // Navigation
        public ICollection<GameScore> Scores { get; set; }
    }
}
