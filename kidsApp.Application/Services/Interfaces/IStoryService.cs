using kidsApp.Application.Dto.StoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.S_Interfaces
{
    public interface IStoryService
    {
        Task<IEnumerable<StoryDTO>> GetAllAsync();
        Task<StoryDTO> GetByIdAsync(int id);
        Task<StoryDTO> CreateAsync(CreateStoryDTO dto);
        Task<bool> UpdateAsync(int id, UpdateStoryDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
