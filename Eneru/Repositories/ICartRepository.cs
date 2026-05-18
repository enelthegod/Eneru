using Eneru.Models;

namespace Eneru.Repositories
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetByUserAsync(int userId);
        Task<CartItem?> GetItemAsync(int userId, int productId);
        Task<CartItem?> GetByIdAsync(int cartItemId, int userId);
        Task AddAsync(CartItem item);
        Task UpdateAsync(CartItem item);
        Task RemoveAsync(CartItem item);
        Task ClearAsync(int userId);
        Task<int> GetCountAsync(int userId);
    }
}