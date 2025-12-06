using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Entites
{
    public class Report
    {
        //public int ReportId { get; set; }
        public int Id { get; set; }
        public int ChildId { get; set; }

        public int GamesPlayed { get; set; }
        public string ReportMonth { get; set; }
        public int TotalPoints { get; set; }
        public double AvgGamesScore { get; set; }
        public int StoriesCompleted { get; set; }
        public int VideosWatched { get; set; }
        public int TasksCompleted { get; set; }

        public Child Child { get; set; }
    }
}
