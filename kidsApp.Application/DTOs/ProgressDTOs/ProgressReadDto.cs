using kidsApp.Application.DTOs.ActivityDTOs;
using kidsApp.Application.DTOs.ChildDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.ProgressDTOs
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
