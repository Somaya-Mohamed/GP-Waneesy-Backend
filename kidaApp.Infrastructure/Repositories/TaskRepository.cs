using kidsApp.Infrastructure.Data;
using kidsApp.Application.Interfaces.Repository;
using kidsApp.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = kidsApp.Domain.Entites.Task;

namespace kidsApp.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {
        public TaskRepository(KidsAppDbContext context) : base(context) { }
    }
}
