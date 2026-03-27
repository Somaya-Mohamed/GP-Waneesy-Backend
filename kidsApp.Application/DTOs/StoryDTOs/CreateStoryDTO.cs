namespace kidsApp.Application.DTOs.StoryDTOs
{
    public class CreateStoryDTO
    {
        public string Title { get; set; } = string.Empty;
        public string StoryText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? AudioUrl { get; set; }
        public string? Url { get; set; }
        public int PointsRewarded { get; set; } = 20;
    }
}