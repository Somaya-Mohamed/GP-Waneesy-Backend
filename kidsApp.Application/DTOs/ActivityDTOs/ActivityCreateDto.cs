using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.ActivityDTOs
{
    public class ActivityCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int RecommendedAge { get; set; }
    }

}
