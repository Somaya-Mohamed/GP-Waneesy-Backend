using kidsApp.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kidsApp.Application.Interfaces;

namespace kidsApp.Application.Interfaces.Repository
{
    public interface IChildRepository : IGenericRepository<Child>
    {
        Task<IEnumerable<Child>> GetChildrenByParentId(int parentId);
    
    }
}
