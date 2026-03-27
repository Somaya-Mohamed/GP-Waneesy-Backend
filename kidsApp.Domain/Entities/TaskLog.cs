using System;

namespace kidsApp.Domain.Entities
{
    public class TaskLog
    {
        public int Id { get; set; }

        public int ChildId { get; set; }
        public int TaskId { get; set; }
        public int PointsEarned { get; set; }

        public string Status { get; set; } = "Pending";
        public DateTime? DateCompleted { get; set; }

        // Navigation
        public Child Child { get; set; } = null!;
        public Tasks Task { get; set; } = null!;
    }
}