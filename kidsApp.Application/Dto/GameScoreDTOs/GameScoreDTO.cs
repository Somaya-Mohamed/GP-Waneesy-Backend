using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Dto.GameScoreDTOs
{
    public class GameScoreDTO
    {
        public int ScoreId { get; set; }
        public int ChildId { get; set; }
        public string ChildName { get; set; }

        public int GameId { get; set; }
        public string GameTitle { get; set; }

        public int Score { get; set; }
        public DateTime Date { get; set; }
    }

}
