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
    public class ArticleRepository : GenericRepository<Article>
    {
        private readonly KidsAppDbContext _context;

        public ArticleRepository(KidsAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> SearchByNameAsync(string name)
        {
            return await _context.Articles
                .Where(a => a.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }
    }
}
