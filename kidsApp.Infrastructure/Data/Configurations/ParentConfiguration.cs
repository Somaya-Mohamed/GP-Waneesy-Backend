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

    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Password)
                   .IsRequired();

            builder.Property(p => p.Country)
                   .HasMaxLength(100);

            builder.Property(p => p.Role)
                   .HasMaxLength(50);

            builder.HasIndex(p => p.Email)
                   .IsUnique(); 
        }
    }
}

