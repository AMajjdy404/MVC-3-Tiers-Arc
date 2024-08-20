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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
           // Fluent APIs For Employee Model

            builder.Property(e =>e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Address).IsRequired();
            builder.Property(e => e.Salary).HasColumnType("decimal(12,2)");
     

            builder.Property(e => e.Gender)
                .HasConversion(
                (GenderDb) => GenderDb.ToString(), // Stored in DB as String
                (GenderApp) => (Gender) Enum.Parse(typeof(Gender),GenderApp)); // Returned from DB in App as Gender (Enum)

            builder.Property(e => e.EmployeeType)
                .HasConversion(
                (EmpDB) => EmpDB.ToString(), // Stored in DB as String
                (EmpApp) => (EmpType) Enum.Parse(typeof(EmpType),EmpApp) // Returned from DB in App as EmpType (Enum)

                );
        }
    }
}
