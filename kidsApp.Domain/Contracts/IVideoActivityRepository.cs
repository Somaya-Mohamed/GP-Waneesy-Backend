using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Contracts
{
    public interface IVideoActivityRepository : IGenericRepository<VideoActivity>
    {
        Task<IEnumerable<VideoActivity>> GetByChildIdAsync(int childId);
        Task<IEnumerable<VideoActivity>> GetByVideoIdAsync(int videoId);
        Task<VideoActivity> GetByIdWithDetailsAsync(int id);
    }
}
