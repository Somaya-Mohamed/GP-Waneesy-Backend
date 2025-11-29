using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Entites
{
    public class Parent
    {
        public int Id { get; set; }
        //public int ParentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string Role { get; set; }

        // Navigation
        public ICollection<Child> Children { get; set; }
    }
}
