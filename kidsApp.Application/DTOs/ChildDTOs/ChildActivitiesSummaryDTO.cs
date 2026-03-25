using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.ChildDTOs
{
    public class ChildActivitiesSummaryDTO
    {
        public int ChildId { get; set; }
        public string FullName { get; set; }

        public int TotalActivities { get; set; }
        public int CompletedActivities { get; set; }
        public double CompletionPercentage { get; set; }

        public int TotalStories { get; set; }
        public int CompletedStories { get; set; }

        public int TotalVideos { get; set; }
        public int CompletedVideos { get; set; }

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }

        public List<RecentActivityDto> RecentActivities { get; set; } = new();
    }

    public class RecentActivityDto
    {
        public string ActivityType { get; set; }   // "Story", "Video", "Task"
        public string Title { get; set; }
        public double Progress { get; set; }       
        public DateTime Date { get; set; }
    }
}