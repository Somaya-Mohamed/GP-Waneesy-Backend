using kidsApp.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace kidsApp.Application.Interfaces.Repository
{
    public interface IGameRepository : IGenericRepository<Game>
    {
    }

}
