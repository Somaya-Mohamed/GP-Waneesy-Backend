using kidsApp.Application.Dto.GameDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameReadDto>> GetAllAsync();
        Task<GameReadDto> GetByIdAsync(int id);
        Task<GameReadDto> CreateAsync(GameCreateDTO dto);
        Task<bool> UpdateAsync(int id, GameUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
