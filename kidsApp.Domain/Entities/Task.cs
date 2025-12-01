using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain
{
    public class Task
    {
        public int Id { get; set; }
        //public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Duration { get; set; }
        public string VideoUrl { get; set; }

        // Navigation
        public ICollection<TaskLog> TaskLogs { get; set; }
    }
}
