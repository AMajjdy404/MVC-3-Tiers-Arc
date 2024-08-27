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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext):base(dbContext) // Ask CLR to create object from dbContext
        {
            
        }
        public IQueryable<Employee> GetEmployeeByName(string name)
        =>  _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower()));
        
    }
}
