using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.GameScoreDTOs
{
    public class GameScoreCreateDTO
    {
        public int ChildId { get; set; }
        public int GameId { get; set; }
        public int Score { get; set; }
    }

}
