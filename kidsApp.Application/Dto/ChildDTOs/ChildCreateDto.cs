using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Dto.ChildDTOs
{
    public class ChildCreateDTO
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public int ParentId { get; set; }
    }

}
