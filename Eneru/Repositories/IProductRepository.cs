using Eneru.Models;

namespace Eneru.Repositories
{
    // Defines all database operations for products
    // Controllers depend on this interface, not on DbContext directly
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(string? category, string? search);
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetFeaturedAsync(int count);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}