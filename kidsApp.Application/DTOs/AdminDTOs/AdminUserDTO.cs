namespace kidsApp.Application.DTOs.AdminDTOs
{
    public class AdminUserDTO
    {
        public string Id { get; set; }       // Identity بيحفظ User ID كـ GUID string
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }     // بيتجيب من GetRolesAsync منفصلة
    }
}