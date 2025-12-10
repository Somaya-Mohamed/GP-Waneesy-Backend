using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Dto.GameScoreDTOs
{
    public class GameScoreUpdateDTO
    {
        public int ScoreValue { get; set; }
        public int Attempts { get; set; }
        // Add other properties as needed based on the GameScore entity
    }
}
