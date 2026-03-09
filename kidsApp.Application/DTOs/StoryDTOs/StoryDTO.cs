using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.StoryDTOs
{
    public class StoryDTO
    {
        public int StoryId { get; set; }
        public string Title { get; set; }
        public string StoryText { get; set; }
        public string Category { get; set; }
        public string? Url { get; set; }

    }

}
