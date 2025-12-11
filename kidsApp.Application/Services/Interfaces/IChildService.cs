using kidsApp.Application.Dto.ChildDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.S_Interfaces
{
    public interface IChildService
    {
        Task<IEnumerable<ChildReadDTO>> GetAllAsync();
        Task<ChildReadDTO> GetByIdAsync(int id);
        Task<ChildReadDTO> CreateAsync(ChildCreateDTO dto);
        Task<bool> UpdateAsync(int id, ChildUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }

}
