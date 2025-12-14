using kidsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Infrastructure.Data.Configurations
{


    public class ChildConfiguration : IEntityTypeConfiguration<Child>
    {
        public void Configure(EntityTypeBuilder<Child> builder)
        {
            // Table name 
            builder.ToTable("Children");

            // Primary Key
            builder.HasKey(c => c.Id);

            // Properties

            // Required fields فقط
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Age)
                   .IsRequired()
                   .HasAnnotation("MinValue", 1)
                   .HasAnnotation("MaxValue", 18);

            builder.Property(c => c.ParentId)
                   .IsRequired();

            // Optional fields
            builder.Property(c => c.Gender)
                   .HasMaxLength(20); // "Male", "Female", "Other"

            builder.Property(c => c.Avatar)
                   .HasMaxLength(1000); // URL 

            builder.Property(c => c.Preferences)
                   .HasMaxLength(2000); // JSON , string 


            // Indexes 
            builder.HasIndex(c => c.ParentId);

            // Relationships
            builder.HasOne(c => c.Parent)
                   .WithMany(p => p.Children)
                   .HasForeignKey(c => c.ParentId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }

}
