using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Entities
{
    public class StoryProgress
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public int StoryId { get; set; }
        public string Status { get; set; }
        public double ProgressPercent { get; set; }
        public DateTime DateCompleted { get; set; } 
        public Child Child { get; set; }
        public Story Story { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
