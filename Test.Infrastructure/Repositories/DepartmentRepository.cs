using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Core.Entities.Business.Departments;
using Test.Core.Interfaces.Repositories;
using Test.Infrastructure.Data;

namespace Test.Infrastructure.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _db;
        public DepartmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(Department entity)
        {
            await _db.Departments.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(Department entity)
        {
            _db.Departments.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _db.Departments.AsNoTracking().OrderBy(d => d.Name).ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _db.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task UpdateAsync(Department entity)
        {
            _db.Departments.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId = 0)
        {
            return await _db.Departments.AnyAsync(d => d.Code == code && d.Id != excludeId);
        }

        public Task<bool> HasEmployeesAsync(int departmentId)
        {
            return _db.Employees.AnyAsync(e => e.DepartmentId == departmentId);
        }
    }
}
