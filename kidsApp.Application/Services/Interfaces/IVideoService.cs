using kidsApp.Application.Dto.VideoDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.S_Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoDTO>> GetAllAsync();
        Task<VideoDTO> GetByIdAsync(int id);
        Task<VideoDTO> CreateAsync(CreateVideoDTO dto);
        Task<bool> UpdateAsync(int id, UpdateVideoDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
