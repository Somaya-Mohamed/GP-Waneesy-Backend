using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Infrastructure.Repositories
{
    public class GameScoreRepository : GenericRepository<GameScore>, IGameScoreRepository
    {
        public GameScoreRepository(KidsAppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<GameScore>> GetWithDetailsAsync()
        {
            return await _dbSet
                .Include(gs => gs.Child)
                .Include(gs => gs.Game)
                .ToListAsync();
        }

        public IQueryable<GameScore> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
