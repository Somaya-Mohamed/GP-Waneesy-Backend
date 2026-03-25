using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace kidsApp.Infrastructure.Persistence
{
    public class KidsAppContextFactory : IDesignTimeDbContextFactory<KidsAppDbContext>
    {
        //public KidsAppDbContext CreateDbContext(string[] args)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<KidsAppDbContext>();
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-B07O4QL\\SQL2024;Database=KidsApp;Trusted_Connection=True;TrustServerCertificate=True");
        //    return new KidsAppDbContext(optionsBuilder.Options);
        //}
        public KidsAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KidsAppDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("Connection");

            optionsBuilder.UseSqlServer(connectionString);

            return new KidsAppDbContext(optionsBuilder.Options);
        }
    }
}