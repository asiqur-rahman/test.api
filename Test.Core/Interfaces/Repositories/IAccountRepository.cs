using System.Threading.Tasks;
using Test.Core.Entities.Common.User;

namespace Test.Core.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int userId);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> UsernameExistsAsync(string username);
        Task<User> CreateAsync(string username, string? email, string password);
    }
}
