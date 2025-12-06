using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Dto.ProgressDTOs
{
    public class ProgressCreateDto
    {
        public int KidId { get; set; }
        public int ActivityId { get; set; }
        public int Score { get; set; }
    }
}
