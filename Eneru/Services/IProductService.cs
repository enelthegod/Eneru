using Eneru.Models;

namespace Eneru.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetCatalogAsync(string? category, string? search);
        Task<Product?> GetDetailAsync(int id);
        Task<List<Product>> GetFeaturedAsync();
        Task CreateAsync(string name, string description, decimal price,
            string brand, int categoryId, string imageUrl);
        Task UpdateAsync(int id, string name, string description, decimal price,
            string brand, int categoryId, string imageUrl, bool isAvailable);
        Task DeleteAsync(int id);
    }
}