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
        public void Add(T Entity)
        {
            _dbContext.Set<T>().Add(Entity);
        }

        public void Update(T Entity)
        {
            _dbContext.Set<T>().Update(Entity);
        }
        public void Delete(T Entity)
        {
            _dbContext.Set<T>().Remove(Entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
                return  (IEnumerable<T>)await _dbContext.Employees.Include(e => e.Department).AsNoTracking().ToListAsync(); // AsNoTracking().ToList() => Immediate Execution
            else
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        => await _dbContext.Set<T>().FindAsync(id);

    }
}
