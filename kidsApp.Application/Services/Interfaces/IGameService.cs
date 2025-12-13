using kidsApp.Application.DTOs.GameDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameReadDto>> GetAllAsync();
        Task<GameReadDto> GetByIdAsync(int id);
        Task<GameReadDto> CreateAsync(GameCreateDTO dto);
        Task<bool> UpdateAsync(int id, GameUpdateDTO dto);
        Task<bool> DeleteAsync(int id);

        // Advanced methods
        Task<IEnumerable<GameReadDto>> GetGamesByDifficultyAsync(string difficulty);
    }
}
