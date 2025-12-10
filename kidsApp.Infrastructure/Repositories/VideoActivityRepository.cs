using kidsApp.Application.Interfaces.Repository;
using kidsApp.Domain.Entites;
using kidsApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Infrastructure.Repositories
{
    public class VideoActivityRepository : GenericRepository<VideoActivity>, IVideoActivityRepository
    {
        public VideoActivityRepository(KidsAppDbContext context) : base(context) { }
    }
}
