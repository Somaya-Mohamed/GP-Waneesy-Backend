using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.VideoActivityDTOs
{
    public class CreateVideoActivityDTO
    {
        public int ChildId { get; set; }
        public int VideoId { get; set; }
        public double WatchPercent { get; set; }
    }

}
