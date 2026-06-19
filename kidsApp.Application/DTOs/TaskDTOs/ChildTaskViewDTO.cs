namespace kidsApp.Application.DTOs.TaskDTOs
{
    /// <summary>
    /// Returned by GET /api/v1/tasks/today
    /// Shows the child their full daily task list (Daily + Personal),
    /// each flagged with whether it was already completed today.
    /// </summary>
    public class ChildTaskViewDTO
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public int PointsRewarded { get; set; }

        /// <summary>"Daily" or "Personal"</summary>
        public string TaskType { get; set; } = string.Empty;

        /// <summary>
        /// True  → already done today (UI shows it as greyed-out / checked).
        /// False → still available to complete.
        /// </summary>
        public bool IsCompletedToday { get; set; }
    }
}