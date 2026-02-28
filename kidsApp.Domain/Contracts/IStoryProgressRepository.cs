using kidsApp.Domain.Entities;

namespace kidsApp.Domain.Contracts
{
    public interface IStoryProgressRepository : IGenericRepository<StoryProgress>
    {
        Task<IEnumerable<StoryProgress>> GetAllWithDetailsAsync();
        Task<StoryProgress?> GetByIdWithDetailsAsync(int id);
    }
}