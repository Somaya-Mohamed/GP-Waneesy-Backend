using System.ComponentModel.DataAnnotations;

namespace kidsApp.Application.DTOs.AdminDTOs
{
    public class AdminCreateUserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}