namespace kidsApp.Application.DTOs.GameDTOs
{
    public class GameReadDto
    {
        public int GameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string GameLink { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int PointsRewarded { get; set; }
    }
}