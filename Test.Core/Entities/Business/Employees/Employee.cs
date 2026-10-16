using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Test.Core.Entities.Business.Departments;

namespace Test.Core.Entities.Business.Employees
{
    [Table("Employees")]
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EmployeeCode { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}