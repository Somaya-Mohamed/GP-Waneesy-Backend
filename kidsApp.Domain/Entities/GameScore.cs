using System;
using System.Collections.Generic;

namespace kidsApp.Domain.Entities
{
    public class GameScore
    {
        public int Id { get; set; }

        public int ChildId { get; set; }
        public int GameId { get; set; }
        public int ScoreValue { get; set; }
        public int Attempts { get; set; } = 1;           // default = 1 

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Navigation
        public Child Child { get; set; } = null!;
        public Game Game { get; set; } = null!;
    }
}