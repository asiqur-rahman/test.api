using Test.Core.Entities.Common.User;
using Test.Core.Interfaces.Repositories;
using Test.Core.Interfaces.Services;

namespace Test.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;

        public AccountService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _accountRepo.GetUserByUsernameAsync(username);
            if (user == null)
                return null;

            bool passwordValid = await _accountRepo.CheckPasswordAsync(user, password);
            return passwordValid ? user : null;
        }

        public async Task<User?> RegisterAsync(string username, string password, string? email)
        {
            if (await _accountRepo.UsernameExistsAsync(username))
                return null;

            return await _accountRepo.CreateAsync(username, email, password);
        }

        public Task<User?> GetUserByUsernameAsync(string username)
            => _accountRepo.GetUserByUsernameAsync(username);

        public Task<User?> GetUserByIdAsync(int userId)
            => _accountRepo.GetUserByIdAsync(userId);
    }
}