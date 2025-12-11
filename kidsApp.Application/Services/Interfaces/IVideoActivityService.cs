using kidsApp.Application.Dto.VideoActivityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.S_Interfaces
{
    public interface IVideoActivityService
    {
        Task<IEnumerable<VideoActivityDTO>> GetAllAsync();
        Task<VideoActivityDTO> GetByIdAsync(int id);
        Task<VideoActivityDTO> CreateAsync(CreateVideoActivityDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
