using kidsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Domain.Contracts
{
    public interface IVideoActivityRepository : IGenericRepository<VideoActivity>
    {
        Task<IEnumerable<VideoActivity>> GetByChildIdWithDetailsAsync(int childId);

        Task<IEnumerable<VideoActivity>> GetByVideoIdWithDetailsAsync(int videoId);

        Task<VideoActivity> GetByIdWithDetailsAsync(int id);

        Task<IEnumerable<VideoActivity>> GetAllWithDetailsAsync();
    }
}