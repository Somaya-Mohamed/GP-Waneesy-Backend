using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.StoryProgress_DTOs
{
    public class StoryProgressDTO
    {
        public int ProgressId { get; set; }

        public int ChildId { get; set; }
        public string ChildName { get; set; }

        public int StoryId { get; set; }
        public string StoryTitle { get; set; }

        public double ProgressPercent { get; set; }
        public DateTime LastUpdated { get; set; }
    }

}
