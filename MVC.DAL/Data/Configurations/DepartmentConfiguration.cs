using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DAL.Data.Configurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Fluent Api For Department Model

            builder.Property(d => d.Id).UseIdentityColumn(10, 10);
            builder.Property(d => d.Code).IsRequired().HasColumnType("varchar").HasMaxLength(50);
            builder.Property(d => d.Name).IsRequired().HasColumnType("varchar").HasMaxLength(50);
        }
    }
}
