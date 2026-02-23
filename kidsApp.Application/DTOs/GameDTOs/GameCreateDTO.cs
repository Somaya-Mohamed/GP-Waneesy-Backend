using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.GameDTOs
{
    public class GameCreateDTO
    {
        public string Title { get; set; }
        public string Difficulty { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string GameLink { get; set; }
        public int PointsRewarded { get; set; }
    }
}
