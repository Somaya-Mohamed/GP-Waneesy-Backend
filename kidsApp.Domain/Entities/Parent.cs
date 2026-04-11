using System.Collections.Generic;

namespace kidsApp.Domain.Entities
{
    public class Parent
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;   
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }

        // Navigation
        public ICollection<Child> Children { get; set; } = new List<Child>();
    }
}