using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Core.Entities.Business.Departments.Dtos;
using Test.Core.Entities.Business.Employees.Dtos;

namespace Test.Core.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(EmployeeDto dto);
        Task UpdateAsync(EmployeeDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync();
    }
}
