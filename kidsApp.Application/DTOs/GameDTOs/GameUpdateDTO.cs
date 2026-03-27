namespace kidsApp.Application.DTOs.GameDTOs
{
    public class GameUpdateDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? GameLink { get; set; }
        public string? Difficulty { get; set; }
        public int? PointsRewarded { get; set; }
    }
}