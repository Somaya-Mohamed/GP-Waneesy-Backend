using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Entities
{
    public class Video
    {
        public int Id { get; set; }
        //public int VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        /// <summary>
        /// //duration will deleted
        /// </summary>
        //public string Duration { get; set; }
        public string VideoUrl { get; set; }

        // Navigation
        public ICollection<VideoActivity> Activities { get; set; }
    }
}
