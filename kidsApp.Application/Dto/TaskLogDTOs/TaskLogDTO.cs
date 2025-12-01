using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Dto.TaskLogDTOs
{
    public class TaskLogDTO
    {
        public int LogId { get; set; }

        public int ChildId { get; set; }
        public string ChildName { get; set; }

        public int TaskId { get; set; }
        public string TaskTitle { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime DateCompleted { get; set; }
    }

}
