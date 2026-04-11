namespace kidsApp.Application.DTOs.ParentDTOs
{
    public class ParentReadDto
    {
        public int ParentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<ChildSummaryDTO> Children { get; set; } = new();
    }
}