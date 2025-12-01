using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Dto.TaskDTOs
{
    public class CreateTaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
    }

}
