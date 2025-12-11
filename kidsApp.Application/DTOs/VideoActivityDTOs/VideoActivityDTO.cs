using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.VideoActivityDTOs
{
    public class VideoActivityDTO
    {
        public int ActivityId { get; set; }

        public int ChildId { get; set; }
        public string ChildName { get; set; }

        public int VideoId { get; set; }
        public string VideoTitle { get; set; }

        public double WatchPercent { get; set; }
        public DateTime LastUpdated { get; set; }
    }

}
