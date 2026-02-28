using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Contracts
{
    public interface IGameScoreRepository : IGenericRepository<GameScore>
    {
        Task<IEnumerable<GameScore>> GetWithDetailsAsync();
    }
}
