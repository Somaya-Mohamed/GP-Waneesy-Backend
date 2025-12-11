using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Domain.Contracts
{
    public interface IChildRepository : IGenericRepository<Child>
    {
        Task<IEnumerable<Child>> GetChildrenByParentId(int parentId);
    
    }
}
