using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

public class KidsAppDbContextFactory : IDesignTimeDbContextFactory<KidsAppDbContext>
{
    public KidsAppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<KidsAppDbContext>();

        // **هنا التعديل:** إزالة المسافات الزائدة
        optionsBuilder.UseSqlServer("Server=DESKTOP-B07O4QL\\SQL2024;Database=KidsApp;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True;");

        return new KidsAppDbContext(optionsBuilder.Options);
    }
}