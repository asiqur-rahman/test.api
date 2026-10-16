using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Core.Entities.Business.Departments;
using Test.Core.Entities.Business.Departments.Dtos;
using Test.Core.Interfaces.Repositories;
using Test.Core.Interfaces.Services;

namespace Test.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repo;

        public DepartmentService(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(ToDto);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : ToDto(entity);
        }

        public async Task<int> CreateAsync(DepartmentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");
            await EnsureCodeIsUnique(dto.Code, 0);

            var entity = new Department
            {
                Name = dto.Name.Trim(),
                Code = dto.Code?.Trim() ?? string.Empty,
                Description = dto.Description?.Trim() ?? string.Empty,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };

            return await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(DepartmentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Department not found.");
            await EnsureCodeIsUnique(dto.Code, dto.Id);

            entity.Name = dto.Name.Trim();
            entity.Code = dto.Code?.Trim() ?? string.Empty;
            entity.Description = dto.Description?.Trim() ?? string.Empty;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.Now;

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Department not found.");
            if (await _repo.HasEmployeesAsync(id))
                throw new InvalidOperationException("Department cannot be deleted while it still has employees.");

            await _repo.DeleteAsync(entity);
        }

        private async Task EnsureCodeIsUnique(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code))
                return;
            if (await _repo.CodeExistsAsync(code.Trim(), excludeId))
                throw new InvalidOperationException("Department code already exists.");
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId = 0)
        {
            return await _repo.CodeExistsAsync(code, excludeId);
        }

        internal static DepartmentDto ToDto(Department d) => new DepartmentDto
        {
            Id = d.Id,
            Name = d.Name,
            Code = d.Code,
            Description = d.Description,
            IsActive = d.IsActive
        };
    }
}