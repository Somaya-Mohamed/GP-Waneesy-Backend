using System.ComponentModel.DataAnnotations;

namespace kidsApp.Application.DTOs.AdminDTOs
{
    public class AdminCreateRoleDTO
    {
        [Required]
        public string Name { get; set; }
    }
}