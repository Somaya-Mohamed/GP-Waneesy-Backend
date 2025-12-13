using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Entities
{
    public class GameScore
    {
        public int Id { get; set; }
        //public int ScoreId { get; set; }
        public int ChildId { get; set; }
        public int GameId { get; set; }

        public int ScoreValue { get; set; }
        public int Attempts { get; set; }

        // Navigation
        public Child Child { get; set; }
        public Game Game { get; set; }
        public DateTime Date { get; set; }
    }
}
