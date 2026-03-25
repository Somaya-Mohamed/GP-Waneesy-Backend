namespace kidsApp.Application.DTOs.ChildDTOs
{
        public class ChildReportDTO
        {
            public int ChildId { get; set; }
            public string FullName { get; set; }
            public int TotalPoints { get; set; }
            public int GamesPlayed { get; set; }
            public int StoriesCompleted { get; set; }
            public int TasksCompleted { get; set; }
        }
    }
