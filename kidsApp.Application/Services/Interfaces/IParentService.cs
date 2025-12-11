using kidsApp.Application.DTOs.ParentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IParentService
    {
        Task<IEnumerable<ParentReadDto>> GetAllAsync();
        Task<ParentReadDto> GetByIdAsync(int id);
        Task<ParentReadDto> CreateAsync(ParentCreateDto dto);
        Task<bool> UpdateAsync(int id, UpdateParentDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
