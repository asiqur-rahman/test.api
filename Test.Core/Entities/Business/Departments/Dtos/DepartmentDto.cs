using System.ComponentModel.DataAnnotations;

namespace Test.Core.Entities.Business.Departments.Dtos
{
    public class DepartmentDto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        public string Code { get; set; } = string.Empty;

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}