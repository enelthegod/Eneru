using Eneru.Data;
using Eneru.Models;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _db;

        public CartRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CartItem>> GetByUserAsync(int userId)
        {
            return await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartItem?> GetItemAsync(int userId, int productId)
        {
            return await _db.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }

        public async Task<CartItem?> GetByIdAsync(int cartItemId, int userId)
        {
            return await _db.CartItems
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);
        }

        public async Task AddAsync(CartItem item)
        {
            _db.CartItems.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(CartItem item)
        {
            _db.CartItems.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(CartItem item)
        {
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task ClearAsync(int userId)
        {
            var items = await _db.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();
            _db.CartItems.RemoveRange(items);
            await _db.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync(int userId)
        {
            return await _db.CartItems
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Quantity);
        }
    }
}