using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Dto.VideoDTOs
{
    public class VideoDTO
    {
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Difficulty { get; set; }
    }

}
