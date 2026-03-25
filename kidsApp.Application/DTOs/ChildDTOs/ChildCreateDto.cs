namespace kidsApp.Application.DTOs.ChildDTOs
{
    public class ChildCreateDTO
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public int ParentId { get; set; }

        public string? AvatarUrl { get; set; }
        public string? Preferences { get; set; }
    }
}