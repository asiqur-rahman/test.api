using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Entities.Common.User;

namespace Test.Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<User?> LoginAsync(string username, string password);
        Task<User?> RegisterAsync(string username, string password, string? email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int userId);
    }
}
