namespace kidsApp.Application.DTOs.GameScoreDTOs
{
    public class GameScoreDTO
    {
        public int ScoreId { get; set; }
        public int ChildId { get; set; }
        public string ChildName { get; set; } = string.Empty;
        public int GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public int Score { get; set; }
        public int Attempts { get; set; }
        public DateTime Date { get; set; }
    }
}
