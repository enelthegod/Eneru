using Eneru.Models;
using Eneru.Repositories;

namespace Eneru.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _users;

        public AccountService(IUserRepository users)
        {
            _users = users;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _users.GetByEmailAsync(email);
            if (user == null) return null;
            if (!PasswordHasher.Verify(password, user.PasswordHash)) return null;
            return user;
        }

        public async Task<User> RegisterAsync(string name, string email, string password)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = PasswordHasher.Hash(password),
                CreatedAt = DateTime.UtcNow
            };
            await _users.AddAsync(user);
            return user;
        }

        public Task<bool> EmailExistsAsync(string email)
            => _users.ExistsAsync(email);
    }
}