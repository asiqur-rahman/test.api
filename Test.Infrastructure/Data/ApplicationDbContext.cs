using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Test.Core.Entities.Business.Departments;
using Test.Core.Entities.Business.Employees;
using Test.Core.Entities.Common.User;

namespace Test.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ILoggerFactory? _loggerFactory;
        
        protected ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            this._loggerFactory = loggerFactory;
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        //public DbQuery<VmAccountInformation> VmAccountInformation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_loggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Apply additional configurations and seed data
            Test.Infrastructure.Data.ApplicationDbContextConfigurations.Configure(builder);
            Test.Infrastructure.Data.ApplicationDbContextConfigurations.SeedData(builder);
        }
    }

}
