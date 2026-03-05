using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.VideoDTOs
{
    public class UpdateVideoDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        //public string? Duration { get; set; }
        public string? Url { get; set; }
    }

}
