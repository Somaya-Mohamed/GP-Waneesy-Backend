using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;





namespace kidsApp.Infrastructure.Data
{
    public class KidsAppDbContext : DbContext
    {
        public KidsAppDbContext(DbContextOptions<KidsAppDbContext> options)
            : base(options)
        {
        }

        // ======================
        //        DbSets
        // ======================
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }

        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryProgress> StoryProgress { get; set; }

        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoActivity> VideoActivities { get; set; }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameScore> GameScores { get; set; }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<Article> Articles { get; set; }
        // ===== Identity Tables =====
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        public DbSet<IdentityUserClaim<string>> UserClaims { get; set; }
        public DbSet<IdentityUserLogin<string>> UserLogins { get; set; }
        public DbSet<IdentityRoleClaim<string>> RoleClaims { get; set; }
        public DbSet<IdentityUserToken<string>> UserTokens { get; set; }


        // ======================
        //  Configurations
        // ======================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ===== Identity Tables =====

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("AspNetUsers");
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("AspNetRoles");
                entity.HasKey(r => r.Id);
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("AspNetUserRoles");
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("AspNetUserClaims");
                entity.HasKey(uc => uc.Id);
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("AspNetUserLogins");
                entity.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("AspNetRoleClaims");
                entity.HasKey(rc => rc.Id);
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("AspNetUserTokens");
                entity.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
            });

            // ===== Your Configurations =====
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChildConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParentConfiguration).Assembly);
        


           
        }


    }
}










