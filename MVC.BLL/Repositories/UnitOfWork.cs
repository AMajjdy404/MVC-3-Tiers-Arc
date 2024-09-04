using MVC.BLL.Interfaces;
using MVC.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IEmployeeRepository EmployeeRepository { get; set; } = null;
        public IDepartmentRepository DepartmentRepository { get; set; } = null;
        public UnitOfWork(ApplicationDbContext dbContext) // Ask CLR for Creating Object from 'DbContext'
        {
            _dbContext = dbContext;
            EmployeeRepository = new EmployeeRepository(_dbContext);
            DepartmentRepository = new DepartmentRepository(_dbContext);
        }
        public async Task<int> CompleteAsync()
        => await _dbContext.SaveChangesAsync();
          
        public async ValueTask DisposeAsync()
        {
           await _dbContext.DisposeAsync();
        }
    }
}
