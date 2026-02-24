using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Contracts
{

    public interface IGenericRepository<T> where T : class
    {
        //Task<IEnumerable<T>> GetAllAsync();
        //Task<T?> GetByIdAsync(int id);
        //Task AddAsync(T entity);
        //Task UpdateAsync(T entity);
        //Task DeleteAsync(T entity);

        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        void Delete(T entity);
    }
}