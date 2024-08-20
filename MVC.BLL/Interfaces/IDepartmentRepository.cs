using MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        IQueryable<Department> GetDepartmentByName(string name);
    }
}
