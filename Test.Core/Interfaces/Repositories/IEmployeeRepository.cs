using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Core.Entities.Business.Departments;
using Test.Core.Entities.Business.Employees;

namespace Test.Core.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<int> CreateAsync(Employee entity);
        Task UpdateAsync(Employee entity);
        Task DeleteAsync(Employee entity);
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<bool> DepartmentExistsAsync(int departmentId);
    }
}
