using kidsApp.Application.DTOs.ChildDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;

namespace kidsApp.Application.Services.Interfaces
{
    public interface IChildService
    {
        // Basic CRUD
        Task<IEnumerable<ChildReadDTO>> GetAllAsync();
        Task<ChildReadDTO> GetByIdAsync(int id);
        Task<ChildReadDTO> CreateAsync(ChildCreateDTO dto);
        Task<bool> UpdateAsync(int id, ChildUpdateDto dto);
        Task<bool> DeleteAsync(int id);

        // Advanced Methods
        Task<IEnumerable<ProgressReadDto>> GetWeeklyProgressAsync(int childId);
        Task<int> GetTotalPointsAsync(int childId);
        Task<double> GetCompletionPercentageAsync(int childId);
        Task<ChildReportDTO> GetWeeklyReportAsync(int childId);
        Task<ChildActivitiesSummaryDTO> GetChildActivitiesSummaryAsync(int childId);
        Task<IEnumerable<ChildTopScoreDTO>> GetTopScoresAsync(int childId, int topCount = 5);
    }
}
