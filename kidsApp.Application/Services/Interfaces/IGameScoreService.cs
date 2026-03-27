using kidsApp.Application.DTOs.GameScoreDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IGameScoreService
    {
        Task<IEnumerable<GameScoreDTO>> GetAllAsync();
        Task<GameScoreDTO?> GetByIdAsync(int id);
        Task<GameScoreDTO> CreateAsync(GameScoreCreateDTO dto);
        Task<bool> UpdateAsync(int id, GameScoreUpdateDTO dto);
        Task<bool> DeleteAsync(int id);

        // Advanced
        Task<IEnumerable<GameScoreDTO>> GetScoresByGameIdAsync(int gameId);
        Task<IEnumerable<GameScoreDTO>> GetScoresByChildIdAsync(int childId);   // مهم جدًا
        Task<IEnumerable<GameScoreDTO>> GetTopScoresAsync(int topCount);
    }
}