namespace kidsApp.Application.DTOs.AuthDTOs
{
    public class ChildLoginDto
    {
        public int ChildId { get; set; }
        public string PinCode { get; set; } = string.Empty;  
    }
}