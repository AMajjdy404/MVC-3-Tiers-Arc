using Microsoft.EntityFrameworkCore;
using MVC.BLL.Interfaces;
using MVC.DAL.Data;
using MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> , IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext dbContext):base(dbContext) // Ask CLR to create object from dbContext
        {
            
        }
        public IQueryable<Department> GetDepartmentByName(string name)
        {
            return _dbContext.Departments.Where(d => d.Name == name);
        }
    }
}
