using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.DTOs.ParentDTOs
{
    public class ParentReadDto
    {
        public int ParentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<ChildSummaryDTO> Children { get; set; }
    }
}
