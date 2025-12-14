using kidsApp.Domain.Contracts;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace kidsApp.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly KidsAppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(KidsAppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
            => await _dbSet.ToListAsync(ct);

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _dbSet.FindAsync(new object[] { id }, ct);

        public async Task AddAsync(T entity, CancellationToken ct = default)
            => await _dbSet.AddAsync(entity, ct);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);
    }
}


//public class GenericRepository<T> : IGenericRepository<T> where T : class
//{
//    private readonly KidsAppDbContext _context;
//    private readonly DbSet<T> _dbSet;

//    public GenericRepository(KidsAppDbContext context)
//    {
//        _context = context;
//        _dbSet = _context.Set<T>();
//    }

//    public async Task<IEnumerable<T>> GetAllAsync()
//        => await _dbSet.ToListAsync();

//    public async Task<T?> GetByIdAsync(int id)
//        => await _dbSet.FindAsync(id);

//    public async Task AddAsync(T entity)
//        => await _dbSet.AddAsync(entity);

//    public Task UpdateAsync(T entity)
//    {
//        _dbSet.Update(entity);
//        return Task.CompletedTask;
//    }

//    public Task DeleteAsync(T entity)
//    {
//        _dbSet.Remove(entity);
//        return Task.CompletedTask;
//    }
//}
