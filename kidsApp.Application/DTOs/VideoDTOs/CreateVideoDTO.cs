namespace kidsApp.Application.DTOs.VideoDTOs
{
    public class CreateVideoDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int PointsRewarded { get; set; }
    }
}