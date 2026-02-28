using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Infrastructure.Repositories
{
    public class GameScoreRepository : GenericRepository<GameScore>, IGameScoreRepository
    {
        public GameScoreRepository(KidsAppDbContext context) : base(context) { }

        public async Task<IEnumerable<GameScore>> GetWithDetailsAsync()
        {
            return await _context.GameScores
                .Include(x => x.Game)
                .Include(x => x.Child)
                .ToListAsync();
        }
    }
}
