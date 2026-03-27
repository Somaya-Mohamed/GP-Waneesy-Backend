using kidsApp.Application.DTOs.StoryDTOs;
using kidsApp.Application.DTOs.StoryProgress_DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IStoryService
    {
        Task<IEnumerable<StoryDTO>> GetAllAsync();
        Task<StoryDTO> GetByIdAsync(int id);
        Task<StoryDTO> CreateAsync(CreateStoryDTO dto);
        Task<bool> UpdateAsync(int id, UpdateStoryDTO dto);
        Task<bool> DeleteAsync(int id);

        // Advanced Methods
        //Task<IEnumerable<StoryProgressDTO>> GetStoryProgressByIdAsync(int storyId);
        Task<IEnumerable<StoryDTO>> GetStoriesByCategoryAsync(string category);
    }
}
