namespace kidsApp.Application.DTOs.TaskLogDTOs
{
    public class TaskLogDTO
    {
        public int LogId { get; set; }
        public int ChildId { get; set; }
        public string ChildName { get; set; } = string.Empty;
        public int TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public int PointsEarned { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DateCompleted { get; set; }
    }
}