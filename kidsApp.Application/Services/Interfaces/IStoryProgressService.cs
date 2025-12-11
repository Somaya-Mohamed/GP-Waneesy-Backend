using kidsApp.Application.Dto.StoryProgress_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.S_Interfaces
{
    public interface IStoryProgressService
    {
        Task<IEnumerable<StoryProgressDTO>> GetAllAsync();
        Task<StoryProgressDTO> GetByIdAsync(int id);
        Task<StoryProgressDTO> CreateAsync(CreateStoryProgressDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
