using System;

namespace kidsApp.Domain.Entities
{
    public class VideoActivity
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public int VideoId { get; set; }
        public double WatchedPercent { get; set; }
        public string Status { get; set; } = "In Progress";

        public DateTime? LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation
        public Child Child { get; set; } = null!;
        public Video Video { get; set; } = null!;
    }
}