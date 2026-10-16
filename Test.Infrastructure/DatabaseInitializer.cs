using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.Infrastructure.Data;

namespace Test.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.EnsureCreatedAsync();
        }
    }
}
