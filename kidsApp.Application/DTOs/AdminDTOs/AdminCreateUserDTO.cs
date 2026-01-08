namespace kidsApp.Application.DTOs.AdminDTOs
{
    public class AdminCreateUserDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // hashed in service
        public string Role { get; set; }
    }
}
