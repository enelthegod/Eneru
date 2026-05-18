using Eneru.Models;

namespace Eneru.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsAsync(string email);
        Task<int> CountAsync();
        Task AddAsync(User user);
    }
}