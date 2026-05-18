using Eneru.Models;
using Eneru.Repositories;

namespace Eneru.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _products;

        public ProductService(IProductRepository products)
        {
            _products = products;
        }

        public Task<List<Product>> GetCatalogAsync(string? category, string? search)
            => _products.GetAllAsync(category, search);

        public Task<Product?> GetDetailAsync(int id)
            => _products.GetByIdAsync(id);

        public Task<List<Product>> GetFeaturedAsync()
            => _products.GetFeaturedAsync(8);

        public Task CreateAsync(string name, string description, decimal price,
            string brand, int categoryId, string imageUrl)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Brand = brand,
                CategoryId = categoryId,
                ImageUrl = string.IsNullOrEmpty(imageUrl) ? "/images/placeholder.jpg" : imageUrl,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };
            return _products.AddAsync(product);
        }

        public async Task UpdateAsync(int id, string name, string description, decimal price,
            string brand, int categoryId, string imageUrl, bool isAvailable)
        {
            var product = await _products.GetByIdAsync(id);
            if (product == null) return;

            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.Brand = brand;
            product.CategoryId = categoryId;
            product.ImageUrl = imageUrl;
            product.IsAvailable = isAvailable;

            await _products.UpdateAsync(product);
        }

        public Task DeleteAsync(int id) => _products.DeleteAsync(id);
    }
}