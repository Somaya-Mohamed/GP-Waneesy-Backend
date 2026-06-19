using System.Collections.Generic;

namespace kidsApp.Domain.Entities
{
    /// TaskType = "Daily"    → Admin-created tasks shown to ALL children every day (reset at midnight)
    /// TaskType = "Personal" → Child-created tasks visible ONLY to that child (also reset at midnight)
    public class Tasks
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = "Easy";

        public string Duration { get; set; } = "10";
        public string? VideoUrl { get; set; }

        public int PointsRewarded { get; set; }

        /// "Daily" = fixed task created by Admin, shown to every child every day.
        /// "Personal" = task created by a specific Child, visible only to them.
        public string TaskType { get; set; } = "Daily";

        public int? CreatedByChildId { get; set; }

        // Navigation
        public ICollection<TaskLog> TaskLogs { get; set; } = new List<TaskLog>();
    }
}