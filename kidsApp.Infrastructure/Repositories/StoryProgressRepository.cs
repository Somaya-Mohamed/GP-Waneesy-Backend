using kidsApp.Infrastructure.Data;
using kidsApp.Application.Interfaces.Repository;
using kidsApp.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Infrastructure.Repositories
{
    public class StoryProgressRepository : GenericRepository<StoryProgress>, IStoryProgressRepository
    {
        public StoryProgressRepository(KidsAppDbContext context) : base(context) { }
    }
}
