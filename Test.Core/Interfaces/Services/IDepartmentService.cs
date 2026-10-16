using Test.Core.Entities.Business.Departments.Dtos;

namespace Test.Core.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(DepartmentDto dto);
        Task UpdateAsync(DepartmentDto dto);
        Task DeleteAsync(int id);
        Task<bool> CodeExistsAsync(string code, int excludeId = 0);
    }
}