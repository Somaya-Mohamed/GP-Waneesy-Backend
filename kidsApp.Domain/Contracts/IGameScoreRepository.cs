using kidsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Domain.Contracts
{
    public interface IGameScoreRepository : IGenericRepository<GameScore>
    {
        Task<IReadOnlyList<GameScore>> GetWithDetailsAsync();

        IQueryable<GameScore> GetQueryable();
    }
}