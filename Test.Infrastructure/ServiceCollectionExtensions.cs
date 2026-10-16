using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Test.Infrastructure.Data;
using Test.Core.Interfaces.Repositories;
using Test.Infrastructure.Repositories;

namespace Test.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectInfrastructure(this IServiceCollection services, string connectionString)
        {
            // Register DbContext using either SQL Server (when connection string provided) or InMemory for local/dev
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (!string.IsNullOrWhiteSpace(connectionString) && connectionString != "InMemory")
                {
                    options.UseSqlServer(connectionString);
                }
                else
                {
                    options.UseInMemoryDatabase("TestInMemoryDb");
                }
            });

            // Repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }
    }
}
