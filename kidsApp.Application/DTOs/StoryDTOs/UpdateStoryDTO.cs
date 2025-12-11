using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.StoryDTOs
{
    public class UpdateStoryDTO
    {
        public string Title { get; set; }
        public string StoryText { get; set; }
        public string Difficulty { get; set; }
    }

}
