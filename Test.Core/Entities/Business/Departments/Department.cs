using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Test.Core.Entities.Business.Employees;

namespace Test.Core.Entities.Business.Departments
{
    [Table("Departments")]
    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        public string Code { get; set; } = string.Empty;

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}