using Microsoft.Extensions.DependencyInjection;
using Test.Core.Interfaces.Services;
using Test.Application.Services;

namespace Test.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            return services;
        }
    }
}
