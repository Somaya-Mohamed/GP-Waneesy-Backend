namespace kidsApp.Application.DTOs.TaskDTOs
{
    public class TaskDTO
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public int PointsRewarded { get; set; }
    }
}