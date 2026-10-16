using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Core.Entities.Business.Departments;

namespace Test.Core.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<int> CreateAsync(Department entity);
        Task UpdateAsync(Department entity);
        Task DeleteAsync(Department entity);
        Task<bool> CodeExistsAsync(string code, int excludeId = 0);
        Task<bool> HasEmployeesAsync(int departmentId);
    }
}
