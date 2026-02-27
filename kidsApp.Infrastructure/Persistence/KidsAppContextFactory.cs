using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace kidsApp.Infrastructure.Persistence
{
    public class KidsAppContextFactory : IDesignTimeDbContextFactory<KidsAppDbContext>
    {
        public KidsAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KidsAppDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-B07O4QL\\SQL2024;Database=KidsApp;Trusted_Connection=True;TrustServerCertificate=True");
            return new KidsAppDbContext(optionsBuilder.Options);
        }
    }
}