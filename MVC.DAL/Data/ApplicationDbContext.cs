using Microsoft.EntityFrameworkCore;
using MVC.DAL.Data.Configurations;
using MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DAL.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            

        }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new DepartmentConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

       public DbSet<Department> Departments { get; set; }
       public DbSet<Employee> Employees { get; set; }
    }
}
