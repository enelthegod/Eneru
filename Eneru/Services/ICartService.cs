using Eneru.Models;

namespace Eneru.Services
{
    public interface ICartService
    {
        Task<List<CartItem>> GetCartAsync(int userId);
        Task AddToCartAsync(int userId, int productId, int quantity);
        Task RemoveFromCartAsync(int cartItemId, int userId);
        Task UpdateQuantityAsync(int cartItemId, int userId, int quantity);
        Task<int> GetCartCountAsync(int userId);
        Task ClearCartAsync(int userId);
    }
}