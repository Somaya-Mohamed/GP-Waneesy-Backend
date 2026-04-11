using System.Collections.Generic;

namespace kidsApp.Domain.Entities
{
    public class Video
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public string VideoUrl { get; set; } = string.Empty;   

        public int PointsRewarded { get; set; }

        // Navigation
        public ICollection<VideoActivity> Activities { get; set; } = new List<VideoActivity>();
    }
}