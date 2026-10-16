using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Core.Interfaces.Repositories;
using Test.Infrastructure.Data;
using Test.Core.Entities.Business.Employees;
using Test.Core.Entities.Business.Departments;

namespace Test.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;
        public EmployeeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(Employee entity)
        {
            await _db.Employees.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(Employee entity)
        {
            entity.Department = null;
            _db.Employees.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _db.Employees.AsNoTracking().Include(e => e.Department).OrderBy(e => e.Name).ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _db.Employees.AsNoTracking().Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(Employee entity)
        {
            entity.Department = null;
            _db.Employees.Update(entity);
            await _db.SaveChangesAsync();
        }

        public Task<bool> DepartmentExistsAsync(int departmentId)
        {
            return _db.Departments.AnyAsync(d => d.Id == departmentId && d.IsActive);
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await _db.Departments.AsNoTracking().Where(d => d.IsActive).OrderBy(d => d.Name).ToListAsync();
        }
    }
}
