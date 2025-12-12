using kidsApp.Application.DTOs.VideoDTOs;
using kidsApp.Application.DTOs.VideoActivityDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoDTO>> GetAllAsync();
        Task<VideoDTO> GetByIdAsync(int id);
        Task<VideoDTO> CreateAsync(CreateVideoDTO dto);
        Task<bool> UpdateAsync(int id, UpdateVideoDTO dto);
        Task<bool> DeleteAsync(int id);

        // Advanced Methods
        Task<IEnumerable<VideoActivityDTO>> GetVideoActivitiesByIdAsync(int videoId);
        Task<IEnumerable<VideoDTO>> GetVideosByDifficultyAsync(string level);
        Task<IEnumerable<VideoDTO>> GetTopWatchedVideosAsync(int topCount = 5);
    }
}
