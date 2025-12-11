using kidsApp.Application.DTOs.ChildDTOs;
using kidsApp.Application.DTOs.GameScoreDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IGameScoreService
    {
        Task<IEnumerable<GameScoreDTO>> GetAllAsync();
        Task<GameScoreDTO> GetByIdAsync(int id);
        Task<GameScoreDTO> CreateAsync(GameScoreCreateDTO dto);
        Task<bool> UpdateAsync(int id,GameScoreUpdateDTO dto);
        Task<bool> DeleteAsync(int id);

    }

}
