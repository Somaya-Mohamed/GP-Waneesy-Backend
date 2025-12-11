using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using kidsApp.Domain.Entites;





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

        public DbSet<Domain.Entites.Tasks> Tasks { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }

        public DbSet<Report> Reports { get; set; }


        // ======================
        //  Fluent API Relations
        // ======================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --------------------------------------
            // Parent → Children  (1 : Many)
            // --------------------------------------
            modelBuilder.Entity<Parent>()
                .HasMany(p => p.Children)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Cascade);


            // --------------------------------------
            // Child → StoryProgress (1 : Many)
            // --------------------------------------
            modelBuilder.Entity<StoryProgress>()
                .HasOne(sp => sp.Child)
                .WithMany(c => c.StoryProgress)
                .HasForeignKey(sp => sp.ChildId);

            modelBuilder.Entity<StoryProgress>()
                .HasOne(sp => sp.Story)
                .WithMany(s => s.StoryProgress)
                .HasForeignKey(sp => sp.StoryId);


            // --------------------------------------
            // Child → VideoActivity (1 : Many)
            // --------------------------------------
            modelBuilder.Entity<VideoActivity>()
                .HasOne(va => va.Child)
                .WithMany(c => c.VideoActivities)
                .HasForeignKey(va => va.ChildId);

            modelBuilder.Entity<VideoActivity>()
                .HasOne(va => va.Video)
                .WithMany(v => v.Activities)
                .HasForeignKey(va => va.VideoId);


            // --------------------------------------
            // Child → GameScore (1 : Many)
            // --------------------------------------
            modelBuilder.Entity<GameScore>()
                .HasOne(gs => gs.Child)
                .WithMany(c => c.GameScores)
                .HasForeignKey(gs => gs.ChildId);

            modelBuilder.Entity<GameScore>()
                .HasOne(gs => gs.Game)
                .WithMany(g => g.Scores)
                .HasForeignKey(gs => gs.GameId);


            // --------------------------------------
            // Child → TaskLog (1 : Many)
            // --------------------------------------
            modelBuilder.Entity<TaskLog>()
                .HasOne(tl => tl.Child)
                .WithMany(c => c.TaskLogs)
                .HasForeignKey(tl => tl.ChildId);

            modelBuilder.Entity<TaskLog>()
                .HasOne(tl => tl.Task)
                .WithMany(t => t.TaskLogs)
                .HasForeignKey(tl => tl.TaskId);


            // --------------------------------------
            // Child → Report (1 : Many)
            // --------------------------------------
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Child)
                .WithMany(c => c.Reports)
                .HasForeignKey(r => r.ChildId);

            // Optional: Composite Keys (لو تحبين)
            // modelBuilder.Entity<StoryProgress>()
            //     .HasKey(sp => new { sp.ChildId, sp.StoryId });
        }
    }
}




