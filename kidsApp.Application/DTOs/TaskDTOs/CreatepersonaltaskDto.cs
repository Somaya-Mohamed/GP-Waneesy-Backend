
namespace kidsApp.Application.DTOs.TaskDTOs
{
    /// <summary>
    /// Used by a Child to create a Personal task visible only to them.
    /// TaskType = "Personal" and CreatedByChildId are set by the service from the JWT — not sent by the client.
    /// </summary>
    public class CreatePersonalTaskDTO
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