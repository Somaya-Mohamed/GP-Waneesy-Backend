using kidsApp.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Interfaces.Repository
{
    public interface IParentRepository : IGenericRepository<Parent>
    {
        Task<Parent?> GetParentWithChildren(int parentId);

    }
}
