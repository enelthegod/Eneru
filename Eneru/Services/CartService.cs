using Eneru.Models;
using Eneru.Repositories;

namespace Eneru.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cart;

        public CartService(ICartRepository cart)
        {
            _cart = cart;
        }

        public Task<List<CartItem>> GetCartAsync(int userId)
            => _cart.GetByUserAsync(userId);

        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            var existing = await _cart.GetItemAsync(userId, productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
                await _cart.UpdateAsync(existing);
                return;
            }

            await _cart.AddAsync(new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            });
        }

        public async Task RemoveFromCartAsync(int cartItemId, int userId)
        {
            var item = await _cart.GetByIdAsync(cartItemId, userId);
            if (item != null)
                await _cart.RemoveAsync(item);
        }

        public async Task UpdateQuantityAsync(int cartItemId, int userId, int quantity)
        {
            var item = await _cart.GetByIdAsync(cartItemId, userId);
            if (item == null) return;

            if (quantity <= 0)
                await _cart.RemoveAsync(item);
            else
            {
                item.Quantity = quantity;
                await _cart.UpdateAsync(item);
            }
        }

        public Task<int> GetCartCountAsync(int userId)
            => _cart.GetCountAsync(userId);

        public Task ClearCartAsync(int userId)
            => _cart.ClearAsync(userId);
    }
}