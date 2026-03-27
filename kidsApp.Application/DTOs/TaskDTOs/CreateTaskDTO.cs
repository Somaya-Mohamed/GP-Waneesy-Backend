namespace kidsApp.Application.DTOs.TaskDTOs
{
    public class CreateTaskDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = "Easy";
        public string Duration { get; set; } = "10";
        public string? VideoUrl { get; set; }
        public int PointsRewarded { get; set; } = 10;
    }
}