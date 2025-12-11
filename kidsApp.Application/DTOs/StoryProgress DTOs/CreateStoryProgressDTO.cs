using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.StoryProgress_DTOs
{
    public class CreateStoryProgressDTO
    {
        public int ChildId { get; set; }
        public int StoryId { get; set; }
        public double ProgressPercent { get; set; }
    }

}
