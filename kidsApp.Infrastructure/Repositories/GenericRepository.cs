using kidsApp.Domain.Contracts;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace kidsApp.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly KidsAppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(KidsAppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }
    }
}



