namespace kidsApp.Application.DTOs.TaskDTOs
{
    public class UpdateTaskDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Difficulty { get; set; }
        public string? Duration { get; set; }
        public string? VideoUrl { get; set; }
        public int? PointsRewarded { get; set; }
    }
}