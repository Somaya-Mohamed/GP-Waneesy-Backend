using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Entities
{
    public class TaskLog
    {
        public int Id { get; set; }
        //public int ActivityId { get; set; }
        public int ChildId { get; set; }
        public int TaskId { get; set; }

        public int PointsEarned { get; set; }
        public string Status { get; set; }

        // Navigation
        public Child Child { get; set; }
        public Tasks Task { get; set; }
    }
}
