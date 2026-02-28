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
    public class StoryRepository : GenericRepository<Story>
    {
        private readonly KidsAppDbContext _context;

        public StoryRepository(KidsAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Story>> GetByCategoryAsync(string category)
        {
            return await _context.Stories
                .Where(s => s.Category.ToLower() == category.ToLower())
                .ToListAsync();
        }
    }
}
