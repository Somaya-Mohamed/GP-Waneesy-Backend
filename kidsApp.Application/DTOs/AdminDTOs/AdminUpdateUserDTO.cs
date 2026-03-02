using System.ComponentModel.DataAnnotations;

namespace kidsApp.Application.DTOs.AdminDTOs
{
    public class AdminUpdateUserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        public string Role { get; set; }

    }
}
