using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.TaskLogDTOs
{
    public class CreateTaskLogDTO
    {
        public int ChildId { get; set; }
        public int TaskId { get; set; }
        public bool IsCompleted { get; set; }
    }

}
