using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Entities
{
    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? AvatarUrl { get; set; }           
        public string? CartoonAvatarUrl { get; set; }    
        public string? Preferences { get; set; }
        public string PinCode { get; set; } = string.Empty;

        // Foreign Key
        public int ParentId { get; set; }
        public Parent Parent { get; set; }

        // Navigation
        public ICollection<StoryProgress> StoryProgress { get; set; }
        public ICollection<VideoActivity> VideoActivities { get; set; }
        public ICollection<GameScore> GameScores { get; set; }
        public ICollection<TaskLog> TaskLogs { get; set; }
        //public object Reports { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
