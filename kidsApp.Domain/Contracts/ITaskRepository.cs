using kidsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Domain.Contracts
{
    public interface ITaskRepository : IGenericRepository<Tasks>
    {
        Task<IEnumerable<Tasks>> GetByCategoryAsync(string category);
    }
}