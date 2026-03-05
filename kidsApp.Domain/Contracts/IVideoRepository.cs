using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Contracts
{
    public interface IVideoRepository : IGenericRepository<Video>
    {
        Task<IEnumerable<Video>> GetByCategoryAsync(string level);
        Task<IEnumerable<Video>> GetTopWatchedAsync(int topCount);
    }
}
