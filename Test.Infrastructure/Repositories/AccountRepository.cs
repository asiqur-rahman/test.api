using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Core.Entities.Common.User;
using Test.Core.Interfaces.Repositories;
using Test.Infrastructure.Data;

namespace Test.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var normalized = username.Trim().ToLower();
            return await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == normalized);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var result = await _db.Users.FindAsync(userId);
            return result;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (user == null) return false;
            var hashString = HashPassword(password);
            return string.Equals(hashString, user.PasswordHash, System.StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            var normalized = username.Trim().ToLower();
            return await _db.Users.AnyAsync(u => u.UserName.ToLower() == normalized);
        }

        public async Task<User> CreateAsync(string username, string? email, string password)
        {
            var user = new User
            {
                UserName = username.Trim(),
                Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
                PasswordHash = HashPassword(password),
                CreatedAt = DateTime.UtcNow
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        private static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hashBytes = sha.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }
    }
}
