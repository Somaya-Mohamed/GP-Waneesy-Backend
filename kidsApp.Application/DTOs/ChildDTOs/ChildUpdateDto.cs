namespace kidsApp.Application.DTOs.ChildDTOs
{
    public class ChildUpdateDto
    {
        public string? Name { get; set; }        
        public int? Age { get; set; }

        public string? AvatarUrl { get; set; }
        public string? Preferences { get; set; }
    }
}