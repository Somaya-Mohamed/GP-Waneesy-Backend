using kidsApp.Application.Dto.ActivityDTOs;
using kidsApp.Application.Dto.ChildDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Dto.ProgressDTOs
{
    public class ProgressReadDto
    {
        public int Id { get; set; }
        public ChildReadDTO Kid { get; set; }
        public ActivityReadDto Activity { get; set; }
        public int Score { get; set; }
        public DateTime DateCompleted { get; set; }
    }
}
