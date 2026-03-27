namespace kidsApp.Application.DTOs.StoryDTOs
{
    public class UpdateStoryDTO
    {
        public string? Title { get; set; }
        public string? StoryText { get; set; }
        public string? Category { get; set; }
        public string? AudioUrl { get; set; }
        public string? Url { get; set; }
        public int? PointsRewarded { get; set; }
    }
}