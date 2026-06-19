using Microsoft.AspNetCore.Http;

namespace kidsApp.Application.DTOs.ChildDTOs
{
    public class ChildAvatarUploadDto
    {
        public IFormFile AvatarImage { get; set; }   
    }
}