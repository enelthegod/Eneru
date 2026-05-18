using Eneru.Models;

namespace Eneru.Services
{
    public interface IAccountService
    {
        Task<User?> LoginAsync(string email, string password);
        Task<User> RegisterAsync(string name, string email, string password);
        Task<bool> EmailExistsAsync(string email);
    }
}