using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.GameDTOs
{
    public class GameReadDto
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Difficulty { get; set; }
    }
}
