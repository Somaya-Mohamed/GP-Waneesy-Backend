namespace kidsApp.Application.DTOs.StoryDTOs
{
    public class StoryDTO
    {
        public int StoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string StoryText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? AudioUrl { get; set; }
        public string? Url { get; set; }
        public int PointsRewarded { get; set; }
    }
}