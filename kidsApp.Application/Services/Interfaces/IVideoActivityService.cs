using kidsApp.Application.DTOs.VideoActivityDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IVideoActivityService
    {
        Task<IEnumerable<VideoActivityDTO>> GetAllAsync();
        Task<VideoActivityDTO> GetByIdAsync(int id);
        Task<VideoActivityDTO> CreateAsync(CreateVideoActivityDTO dto);
        Task<bool> DeleteAsync(int id);

        // Advanced Methods
        Task<bool> UpdateProgressAsync(int id, double watchPercent, string status);
        Task<IEnumerable<VideoActivityDTO>> GetByChildIdAsync(int childId);
        Task<IEnumerable<VideoActivityDTO>> GetProgressByVideoIdAsync(int videoId);
    }
}
