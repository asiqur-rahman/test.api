using System.ComponentModel.DataAnnotations;

namespace Test.Core.Entities.Business.Employees.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        public string EmployeeCode { get; set; } = string.Empty;

        [EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "A department is required.")]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}