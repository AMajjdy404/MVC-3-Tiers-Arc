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
    public class GenericRepository<T>: IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(T Entity)
        {
            _dbContext.Set<T>().Add(Entity);
            return _dbContext.SaveChanges();
        }

        public int Update(T Entity)
        {
            _dbContext.Set<T>().Update(Entity);
            return _dbContext.SaveChanges();
        }
        public int Delete(T Entity)
        {
            _dbContext.Set<T>().Remove(Entity);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>)_dbContext.Employees.Include(e => e.Department).AsNoTracking().ToList(); // AsNoTracking().ToList() => Immediate Execution
            else
                return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        public T GetById(int id)
        => _dbContext.Set<T>().Find(id);

    }
}
