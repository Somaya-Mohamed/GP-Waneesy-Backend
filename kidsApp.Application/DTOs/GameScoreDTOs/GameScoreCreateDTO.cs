namespace kidsApp.Application.DTOs.GameScoreDTOs
{
    public class GameScoreCreateDTO
    {
        public int ChildId { get; set; }
        public int GameId { get; set; }
        public int Score { get; set; }          
        public int Attempts { get; set; } = 1;   
    }
}
