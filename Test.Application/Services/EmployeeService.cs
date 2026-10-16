using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Core.Interfaces.Repositories;
using Test.Core.Entities.Business.Employees;
using Test.Core.Entities.Business.Departments.Dtos;
using Test.Core.Entities.Business.Employees.Dtos;
using Test.Core.Interfaces.Services;

namespace Test.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(ToDto);
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : ToDto(entity);
        }

        public async Task<int> CreateAsync(EmployeeDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");
            if (!await _repo.DepartmentExistsAsync(dto.DepartmentId))
                throw new InvalidOperationException("Department does not exist.");

            var entity = new Employee
            {
                Name = dto.Name,
                EmployeeCode = dto.EmployeeCode,
                Email = dto.Email,
                Phone = dto.Phone,
                Designation = dto.Designation,
                DepartmentId = dto.DepartmentId,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };

            return await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(EmployeeDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Employee not found.");
            if (!await _repo.DepartmentExistsAsync(dto.DepartmentId))
                throw new InvalidOperationException("Department does not exist.");

            entity.Name = dto.Name;
            entity.EmployeeCode = dto.EmployeeCode;
            entity.Email = dto.Email;
            entity.Phone = dto.Phone;
            entity.Designation = dto.Designation;
            entity.DepartmentId = dto.DepartmentId;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.Now;

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Employee not found.");

            await _repo.DeleteAsync(entity);
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync()
        {
            var list = await _repo.GetDepartmentsAsync();
            return list.Select(DepartmentService.ToDto);
        }

        internal static EmployeeDto ToDto(Employee e) => new EmployeeDto
        {
            Id = e.Id,
            Name = e.Name,
            EmployeeCode = e.EmployeeCode,
            Email = e.Email,
            Phone = e.Phone,
            Designation = e.Designation,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department?.Name,
            IsActive = e.IsActive
        };
    }
}