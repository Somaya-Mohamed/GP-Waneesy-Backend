namespace kidsApp.Application.DTOs.ChildDTOs
{
    public class ChildReadDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Preferences { get; set; }
    }
}