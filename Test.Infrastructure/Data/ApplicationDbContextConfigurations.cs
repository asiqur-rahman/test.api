using Microsoft.EntityFrameworkCore;
using Test.Core.Entities.Business.Departments;
using Test.Core.Entities.Business.Employees;
using Test.Core.Entities.Common.User;

namespace Test.Infrastructure.Data
{
    public class ApplicationDbContextConfigurations
    {
        // SHA256("admin"), stored as a constant so seed data does not change on every model build.
        private const string AdminPasswordHash = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918";

        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.UserName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.PasswordHash).HasMaxLength(128).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(200);
                entity.HasIndex(u => u.UserName).IsUnique();
            });

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    PasswordHash = AdminPasswordHash,
                    Email = "admin@local",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LastLoginAt = null
                });
        }
    }
}
