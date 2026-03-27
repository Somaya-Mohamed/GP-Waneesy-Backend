using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Entities
{
    public class Story
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string? AudioUrl { get; set; }
        public string? Url { get; set; }
        public int PointsRewarded { get; set; } = 20;   

        // Navigation
        public ICollection<StoryProgress> StoryProgress { get; set; } = new List<StoryProgress>();
    }
}


