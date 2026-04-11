namespace kidsApp.Application.DTOs.VideoDTOs
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string PointsRewarded { get; set; }

    }
}