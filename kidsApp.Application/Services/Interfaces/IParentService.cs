using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;

public interface IParentService
{
    // Basic CRUD
    Task<IEnumerable<ParentReadDto>> GetAllAsync();
    Task<ParentReadDto> GetByIdAsync(int id);
    Task<ParentReadDto> CreateAsync(ParentCreateDto dto);
    Task<bool> UpdateAsync(int id, UpdateParentDTO dto);
    Task<bool> DeleteAsync(int id);

    // Advanced
    Task<IEnumerable<ChildSummaryDTO>> GetChildrenSummaryAsync(int parentId);
    Task<IEnumerable<ProgressReadDto>> GetWeeklyChildReportsAsync(int parentId);
    Task<string?> LoginAsync(string email, string password);
}
